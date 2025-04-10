<?php
require_once dirname(__DIR__, 2) . '/db/db.php';

$sql_users = "CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);";

$sql_user_data = "CREATE TABLE IF NOT EXISTS user_data (
    user_id INT PRIMARY KEY,
    diamond INT DEFAULT 1000 CHECK (diamond >= 0 AND diamond <= 10000),
    heart INT DEFAULT 100 CHECK (heart >= 0 AND heart <= 100),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);";

$sql_login_history = "CREATE TABLE IF NOT EXISTS login_history (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    login_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);";

if (
    $conn->query($sql_users) === TRUE &&
    $conn->query($sql_user_data) === TRUE &&
    $conn->query($sql_login_history) === TRUE
) {
    echo "Tables created successfully!";
} else {
    echo "Error: " . $conn->error;
}

$conn->close();
?>
