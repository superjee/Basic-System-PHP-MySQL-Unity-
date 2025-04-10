<?php
require_once dirname(__DIR__, 1) . '/db/db.php';

// ดึงข้อมูลจาก users + user_data
$sql = "SELECT u.id, u.username, d.diamond, d.heart
        FROM users u
        LEFT JOIN user_data d ON u.id = d.user_id";
$result_users = $conn->query($sql);

// ดึงข้อมูลจาก login_history
$sql2 = "SELECT h.id, h.user_id, h.login_time, u.username
         FROM login_history h
         LEFT JOIN users u ON h.user_id = u.id
         ORDER BY h.login_time DESC";
$result_history = $conn->query($sql2);
?>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>All User Data</title>
    <style>
        body { font-family: sans-serif; margin: 30px; }
        h2 { margin-top: 40px; }
        table { border-collapse: collapse; width: 100%; margin-bottom: 40px; }
        th, td { border: 1px solid #ccc; padding: 8px 12px; text-align: center; }
        th { background-color: #f4f4f4; }
    </style>
</head>
<body>

<h1>📋 User Summary</h1>

<h2>👤 Users & User Data</h2>
<table>
    <thead>
        <tr>
            <th>ID</th>
            <th>Username</th>
            <th>Diamond</th>
            <th>Heart</th>
        </tr>
    </thead>
    <tbody>
        <?php if ($result_users && $result_users->num_rows > 0): ?>
            <?php while($row = $result_users->fetch_assoc()): ?>
                <tr>
                    <td><?= htmlspecialchars($row['id']) ?></td>
                    <td><?= htmlspecialchars($row['username']) ?></td>
                    <td><?= $row['diamond'] ?></td>
                    <td><?= $row['heart'] ?></td>
                </tr>
            <?php endwhile; ?>
        <?php else: ?>
            <tr><td colspan="4">No users found.</td></tr>
        <?php endif; ?>
    </tbody>
</table>

<h2>🕒 Login History</h2>
<table>
    <thead>
        <tr>
            <th>Log ID</th>
            <th>User ID</th>
            <th>Username</th>
            <th>Login Time</th>
        </tr>
    </thead>
    <tbody>
        <?php if ($result_history && $result_history->num_rows > 0): ?>
            <?php while($row = $result_history->fetch_assoc()): ?>
                <tr>
                    <td><?= $row['id'] ?></td>
                    <td><?= $row['user_id'] ?></td>
                    <td><?= htmlspecialchars($row['username']) ?></td>
                    <td><?= $row['login_time'] ?></td>
                </tr>
            <?php endwhile; ?>
        <?php else: ?>
            <tr><td colspan="4">No login records found.</td></tr>
        <?php endif; ?>
    </tbody>
</table>

</body>
</html>

<?php $conn->close(); ?>
