using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Validator : MonoBehaviour
{
    public static bool ValidateUsername(string username, out string errorMessage)
    {
        errorMessage = "";

        if (string.IsNullOrWhiteSpace(username))
        {
            errorMessage = "Username cannot be empty";
            return false;
        }

        if (username.Length < 4)
        {
            errorMessage = "Username must be at least 4 characters long";
            return false;
        }

        // เช็คว่ามีแต่ a-z, A-Z, 0-9 เท่านั้น
        if (!Regex.IsMatch(username, "^[a-zA-Z0-9]+$"))
        {
            errorMessage = "Username must be in English and numbers only";
            return false;
        }

        return true;
    }
    public static bool ValidatePassword(string password, string confirmPassword, out string errorMessage)
    {
        errorMessage = "";

        if (string.IsNullOrWhiteSpace(password))
        {
            errorMessage = "Password cannot be empty";
            return false;
        }

        if (password.Length < 6)
        {
            errorMessage = "Password must be at least 6 characters long";
            return false;
        }

        if (password != confirmPassword)
        {
            errorMessage = "Passwords do not match";
            return false;
        }

        return true;
    }
}
