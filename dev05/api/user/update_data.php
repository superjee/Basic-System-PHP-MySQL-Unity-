<?php
header('Content-Type: application/json');
require_once dirname(__DIR__, 2) . '/db/db.php';
require_once dirname(__DIR__, 2) . '/utils/logger.php';

if ($_SERVER["REQUEST_METHOD"] !== "POST") {
    echo json_encode([
        "success" => false,
        "message" => "Please use POST method to access this API."
    ]);
    exit();
}

$data = json_decode(file_get_contents("php://input"), true);
$userId = (int)($data["user_id"] ?? 0);

if (!$userId) {
    write_debug_log("UPDATE_FAIL", [
        "reason" => "Missing user_id",
        "data" => $data
    ]);

    echo json_encode(["success" => false, "message" => "Missing user_id"]);
    exit();
}

$fieldConfig = [
    "diamond" => ["min" => 0, "max" => 10000],
    "heart"   => ["min" => 0, "max" => 100],
];

$fieldsToUpdate = array_keys($fieldConfig);
$fieldsSQL = [];
$params = [];
$types = "";
$responseData = [];

// ดึงค่าปัจจุบันจาก DB
$selectFields = implode(",", $fieldsToUpdate);
$stmt = $conn->prepare("SELECT $selectFields FROM user_data WHERE user_id = ?");
$stmt->bind_param("i", $userId);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows === 0) {
    write_debug_log("UPDATE_FAIL", [
        "user_id" => $userId,
        "reason" => "User not found"
    ]);

    echo json_encode(["success" => false, "message" => "User not found"]);
    exit();
}

$currentValues = $result->fetch_assoc();
$stmt->close();

// เตรียมค่าที่จะอัปเดต
foreach ($fieldsToUpdate as $field) {
    $addKey = "add_" . $field;
    if (isset($data[$addKey])) {
        $addValue = (int)$data[$addKey];
        $current = (int)$currentValues[$field];
        $new = $current + $addValue;

        $min = $fieldConfig[$field]["min"];
        $max = $fieldConfig[$field]["max"];

        if ($new > $max) {
            write_debug_log("UPDATE_FAIL", [
                "user_id" => $userId,
                "field" => $field,
                "reason" => "Exceeded max limit",
                "value_attempted" => $new,
                "max_allowed" => $max
            ]);

            echo json_encode([
                "success" => false,
                "message" => "ค่า $field เกินขีดจำกัด",
                "current_$field" => $current,
                "max_$field" => $max
            ]);
            exit();
        }

        $new = max($min, $new); // ป้องกันติดลบ
        $fieldsSQL[] = "$field = ?";
        $params[] = $new;
        $types .= "i";
        $responseData[$field] = $new;
    }
}

if (empty($fieldsSQL)) {
    write_debug_log("UPDATE_FAIL", [
        "user_id" => $userId,
        "reason" => "ไม่มีข้อมูลสำหรับอัปเดต",
        "input" => $data
    ]);

    echo json_encode(["success" => false, "message" => "ไม่มีข้อมูลสำหรับอัปเดต"]);
    exit();
}

$params[] = $userId;
$types .= "i";
$sql = "UPDATE user_data SET " . implode(", ", $fieldsSQL) . " WHERE user_id = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param($types, ...$params);

if ($stmt->execute()) {
	$responseData = array_map('intval', $responseData);
	
    $response = [
        "success" => true,
        "message" => "อัปเดตสำเร็จ",
        "data" => $responseData
    ];

    write_debug_log("UPDATE_SUCCESS", [
        "user_id" => $userId,
        "updated_fields" => $responseData
    ]);

    echo json_encode($response);
} else {
    write_debug_log("UPDATE_FAIL", [
        "user_id" => $userId,
        "reason" => "Database update failed"
    ]);

    echo json_encode(["success" => false, "message" => "เกิดข้อผิดพลาดในการอัปเดต"]);
}

$conn->close();
