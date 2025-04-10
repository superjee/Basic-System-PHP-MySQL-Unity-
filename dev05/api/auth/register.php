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
$username = trim($data["username"] ?? '');
$password = trim($data["password"] ?? '');

if (!$username || !$password) {
    write_debug_log("REGISTER_FAIL", [
        "reason" => "Missing username or password",
        "input" => $data
    ]);

    echo json_encode(["success" => false, "message" => "Username or password missing."]);
    exit();
}

$stmt = $conn->prepare("SELECT id FROM users WHERE username = ?");
$stmt->bind_param("s", $username);
$stmt->execute();
$stmt->store_result();

if ($stmt->num_rows > 0) {
    write_debug_log("REGISTER_FAIL", [
        "username" => $username,
        "reason" => "Username already exists"
    ]);

    echo json_encode(["success" => false, "message" => "Username already exists."]);
    exit();
}

$hashedPassword = password_hash($password, PASSWORD_DEFAULT);
$stmt = $conn->prepare("INSERT INTO users (username, password) VALUES (?, ?)");
$stmt->bind_param("ss", $username, $hashedPassword);

if ($stmt->execute()) {
    $userId = $stmt->insert_id;

    $stmt2 = $conn->prepare("INSERT INTO user_data (user_id) VALUES (?)");
    $stmt2->bind_param("i", $userId);
    $stmt2->execute();

    $response = ["success" => true, "message" => "Registered successfully."];

    write_debug_log("REGISTER_SUCCESS", [
        "username" => $username,
        "user_id" => (int)$userId,
        "response" => $response
    ]);

    echo json_encode($response);
} else {
    write_debug_log("REGISTER_FAIL", [
        "username" => $username,
        "reason" => "Database insert failed"
    ]);

    echo json_encode(["success" => false, "message" => "Registration failed."]);
}

$conn->close();
