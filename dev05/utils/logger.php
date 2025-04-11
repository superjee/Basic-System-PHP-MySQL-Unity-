<?php
date_default_timezone_set('Asia/Bangkok');
function write_debug_log($tag, $data)
{
    $logPath = dirname(__DIR__) . "/debug_log.json";
    $maxSize = 1024 * 1024; // 1MB
    $maxEntries = 100;

    $entry = [
        "tag" => $tag,
        "timestamp" => date("Y-m-d H:i:s"),
        "data" => $data
    ];

    $logData = [];

    if (file_exists($logPath)) {
        $json = file_get_contents($logPath);
        $logData = json_decode($json, true) ?? [];

        if (filesize($logPath) > $maxSize) {
            $logData[] = [
                "tag" => "LOG_CLEANUP",
                "timestamp" => date("Y-m-d H:i:s"),
                "data" => "Trimmed to last $maxEntries entries"
            ];
            $logData = array_slice($logData, -$maxEntries);
        }
    }

    $logData[] = $entry;

    file_put_contents($logPath, json_encode($logData, JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE));
}