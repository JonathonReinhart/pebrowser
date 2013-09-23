using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEBrowser
{
    public class FileDialogFilterBuilder
    {
        public FileDialogFilterBuilder() {
            SetDefaults();
        }

        public void SetDefaults() {
            AllFiles = true;
            AllTypesName = "files";
            AllTypes = true;
        }

        /// <summary>Set to true to include an "All Files (*.*)" entry.</summary>
        public bool AllFiles { get; set; }

        /// <summary>
        /// A name for the entry of all specified extensions.
        /// e.g. "pictures" for an "All pictures (*.jpg, *.png, *.bmp)" entry.
        /// </summary>
        public string AllTypesName { get; set; }

        /// <summary>
        /// Set to true to include an entry for all specified extensions.
        /// e.g. "All pictures (*.jpg, *.png, *.bmp)"
        /// </summary>
        public bool AllTypes { get; set; }



        private class Entry
        {
            public string Name { get; private set; }
            public IEnumerable<string> Extensions { get; private set; }

            public Entry(string name, params string[] extensions) {
                Name = name;
                Extensions = extensions;
            }
        }

        private readonly List<Entry> m_entries = new List<Entry>();

        public void AddExtension(string name, params string[] extensions) {
            m_entries.Add(new Entry(name, extensions));
        }

        public string Filter {
            get {
                return GetFilterText();
            }
        }

        private static void AddFilterEntry(StringBuilder sb, string name, IEnumerable<string> extensions) {
            var extsWithDot = extensions.Select(x => "*." + x).ToArray();

            string extsComma = String.Join(", ", extsWithDot);
            string extsSemicolon = String.Join(";", extsWithDot);

            if (sb.Length != 0) sb.Append("|");
            sb.AppendFormat("{0} ({1})|{2}", name, extsComma, extsSemicolon);
        }


        private string GetFilterText() {
            var sb = new StringBuilder();

            var allExts = new List<string>();

            // Each entry
            foreach (var entry in m_entries) {
                allExts.AddRange(entry.Extensions);
                AddFilterEntry(sb, entry.Name, entry.Extensions);
            }

            // All "things"
            if (AllTypes) {
                AddFilterEntry(sb, "All " + AllTypesName, allExts);
            }

            // All Files
            if (AllFiles) {
                AddFilterEntry(sb, "All Files", new []{"*"});
            }

            return sb.ToString();
        }
    }
}
