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
    write_debug_log("LOGIN_FAIL", [
        "reason" => "Missing username or password",
        "input" => $data
    ]);

    echo json_encode(["success" => false, "message" => "Username or password missing."]);
    exit();
}

$stmt = $conn->prepare("SELECT id, password FROM users WHERE username = ?");
$stmt->bind_param("s", $username);
$stmt->execute();
$stmt->store_result();

if ($stmt->num_rows == 1) {
    $stmt->bind_result($userId, $hashedPassword);
    $stmt->fetch();

    if (password_verify($password, $hashedPassword)) {
        $conn->query("INSERT INTO login_history (user_id) VALUES ($userId)");

        $result = $conn->query("SELECT diamond, heart FROM user_data WHERE user_id = $userId");
        $row = $result->fetch_assoc();

        $response = [
			"success" => true,
			"message" => "Login successful.",
			"data" => [
				"user_id" => (int)$userId,
				"diamond" => (int)$row["diamond"],
				"heart" => (int)$row["heart"]
			]
		];

        write_debug_log("LOGIN_SUCCESS", [
            "username" => $username,
            "user_id" => (int)$userId,
            "response" => $response
        ]);

        echo json_encode($response);
    } else {
        write_debug_log("LOGIN_FAIL", [
            "username" => $username,
            "reason" => "Incorrect password"
        ]);

        echo json_encode(["success" => false, "message" => "Incorrect password."]);
    }
} else {
    write_debug_log("LOGIN_FAIL", [
        "username" => $username,
        "reason" => "User not found"
    ]);

    echo json_encode(["success" => false, "message" => "User not found."]);
}

$conn->close();
