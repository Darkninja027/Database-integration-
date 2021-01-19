using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compx323Project
{
    static class FormHelper
    {
        public static Form mainScreen;

        /// <summary>
        /// Formats strings to uppercase letter begining and lowercase rest.
        /// </summary>
        /// <param name="toFormat">String to format.</param>
        /// <returns>Returns string with each word starting with an uppercase and the rest of it lowercase.</returns>
        public static string Capitalize(string toFormat)
        {
            if (toFormat == "") return "";
            string[] strings;
            if ((strings = toFormat.Split(' ')).Length > 1)
            {
                string toReturn = "";
                for (int i = 0; i < strings.Length - 1; i++)
                {
                    toReturn += Capitalize(strings[i]) + " ";
                }
                toReturn += Capitalize(strings[strings.Length - 1]);
                return toReturn;
            }
            return toFormat[0].ToString().ToUpper() + toFormat.Substring(1, toFormat.Length - 1).ToLower();
        }

        /// <summary>
        /// Formats a number without any white space.
        /// </summary>
        /// <param name="toFormat">Number to format.</param>
        /// <returns>Number as string with no whitespace. Returns empty string if not a number.</returns>
        public static string FormatNumber(string toFormat)
        {
            if (toFormat == "") return "";
            string noWhiteSpace = "";
            foreach (char c in toFormat)
            {
                if (char.IsWhiteSpace(c)) continue;
                else if (!char.IsDigit(c))
                {
                    return "";
                }
                noWhiteSpace += c.ToString();
            }
            return noWhiteSpace;
        }

        /// <summary>
        /// Formats word.
        /// </summary>
        /// <param name="toFormat">Word to format.</param>
        /// <returns>Capitalized word. Returns empty string if error.</returns>
        public static string FormatWord(string toFormat)
        {
            if (toFormat == "") return "";
            foreach (char c in toFormat) if (!char.IsLetter(c) && !(c == '-')) return "";
            return Capitalize(toFormat);
        }

        /// <summary>
        /// Formats words.
        /// </summary>
        /// <param name="toFormat">Words to format.</param>
        /// <returns>String of capitalized words. Returns empty string if error.</returns>
        public static string FormatWords(string toFormat)
        {
            if (toFormat == "") return "";
            string words = "", word;
            foreach (string s in toFormat.Split(' '))
            {
                word = FormatWord(s);
                if (word == "") return "";
                words += word + " ";
            }
            return words.Substring(0,words.Length - 1);
        }

        /// <summary>
        /// Formats alphanumeric word.
        /// </summary>
        /// <param name="toFormat">Words to format.</param>
        /// <returns>String of capitalized words. Returns empty string if error.</returns>
        public static string FormatAlphanumericWord(string toFormat)
        {
            if (toFormat == "") return "";
            foreach (char c in toFormat) if (!char.IsLetterOrDigit(c) && !(c == '-')) return "";
            return Capitalize(toFormat);
        }

        /// <summary>
        /// Formats alphanumeric words.
        /// </summary>
        /// <param name="toFormat">Words to format.</param>
        /// <returns>String of capitalized words. Returns empty string if error.</returns>
        public static string FormatAlphanumericWords(string toFormat)
        {
            if (toFormat == "") return "";
            string words = "", word;
            foreach (string s in toFormat.Split(' '))
            {
                word = FormatAlphanumericWord(s);
                if (word == "") return "";
                words += word + " ";
            }
            return words.Substring(0, words.Length - 1);
        }

        /// <summary>
        /// String error.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <param name="error">Error to display.</param>
        /// <returns>Whether or not there is an error.</returns>
        public static bool StringError(string s, string error)
        {
            if (s == "")
            {
                MessageBox.Show(error);
                return true;
            }
            return false;
        }
    }
}
