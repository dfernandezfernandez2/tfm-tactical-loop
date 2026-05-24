namespace Game.Translation {
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class TranslationFileReader {
        private const string _separator = "#$#";
        private const string _comment = "#";

        public static List<Dictionary<string, string>> Read(string path) {
            List<Dictionary<string, string>> results = new();
            TextAsset csvAsset = Resources.Load<TextAsset>(path);
            string csvText = csvAsset.text;
            string[] lines = csvText.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) {
                return results;
            }

            string[] header = lines[0].Split(_separator);
            for (int i = 1; i < lines.Length; i++) {
                if (lines[i].StartsWith(_comment)) {
                    continue;
                }

                string[] row = lines[i].Split(_separator);
                if (row.Length != header.Length) {
                    throw new ArgumentException(
                        $"Invalid CSV row {i + 1} from {path}. Row length {row.Length}, header length {header.Length}");
                }

                Dictionary<string, string> rowData = new();
                for (int column = 0; column < row.Length; column++) {
                    rowData[header[column].Trim()] = row[column];
                }

                results.Add(rowData);
            }

            return results;
        }
    }
}
