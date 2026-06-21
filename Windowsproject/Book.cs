using System;

namespace Windowsproject
{
    public class Book
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }       // "在館中" or "已借出"
        public string Borrower { get; set; }
        public DateTime? DueDate { get; set; }
        public int Rating { get; set; }           // 1–5
        public DateTime AddedDate { get; set; }

        public string Description { get; set; } = "";

        public int RatingCount { get; set; } = 0;   // 累積評分人數

        public int RatingSum { get; set; } = 0;   // 累積總分

        public Book()
        {
            Status = "在館中";
            Borrower = "";
            DueDate = null;
            Rating = 3;
            AddedDate = DateTime.Today;
        }

        // ── CSV 序列化 ────────────────────────────────────────────
        public string ToCsvLine()
        {
            string due = DueDate.HasValue
                ? DueDate.Value.ToString("yyyy-MM-dd") : "";

            return string.Join(",",
                Escape(BookId),
                Escape(Title),
                Escape(Author),
                Escape(Category),
                Escape(Status),
                Escape(Borrower),
                Escape(due),
                Rating.ToString(),
                AddedDate.ToString("yyyy-MM-dd"),
                Escape(Description),
                RatingCount.ToString(),
                RatingSum.ToString()
            );
        }

        // ── CSV 反序列化 ──────────────────────────────────────────
        public static Book FromCsvLine(string line)
        {
            var f = ParseCsv(line);
            if (f.Length < 10) return null;

            return new Book
            {
                BookId = f[0],
                Title = f[1],
                Author = f[2],
                Category = f[3],
                Status = f[4],
                Borrower = f[5],
                DueDate = string.IsNullOrEmpty(f[6])
                                  ? (DateTime?)null
                                  : DateTime.Parse(f[6]),
                Rating = int.TryParse(f[7], out int r) ? r : 3,
                AddedDate = DateTime.TryParse(f[8], out DateTime d)
                                  ? d : DateTime.Today,
                Description = f.Length > 9 ? f[9] : "",
                RatingCount = f.Length > 10 && int.TryParse(f[10], out int rc) ? rc : 0,  // ← 新增
                RatingSum = f.Length > 11 && int.TryParse(f[11], out int rs) ? rs : 0,
            };
        }

        // ── 輔助：逗號跳脫 ───────────────────────────────────────
        private string Escape(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Contains(",") || s.Contains("\""))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        private static string[] ParseCsv(string line)
        {
            var result = new System.Collections.Generic.List<string>();
            var cur = new System.Text.StringBuilder();
            bool inQ = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQ)
                {
                    if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                    { cur.Append('"'); i++; }
                    else if (c == '"') inQ = false;
                    else cur.Append(c);
                }
                else
                {
                    if (c == '"') inQ = true;
                    else if (c == ',') { result.Add(cur.ToString()); cur.Clear(); }
                    else cur.Append(c);
                }
            }
            result.Add(cur.ToString());
            return result.ToArray();
        }
    }
}