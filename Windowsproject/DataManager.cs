using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Windowsproject
{
    public static class DataManager
    {
        private static string _csvPath;
        private static string _imagesDir;

        // ── 初始化路徑 ────────────────────────────────────────────
        // 在 Form1 建構子呼叫一次
        public static void Initialize()
        {
            string appDir = Application.StartupPath;
            _csvPath = Path.Combine(appDir, "Resources\\Books.csv");
            _imagesDir = Path.Combine(appDir, "covers");



            if (!Directory.Exists(_imagesDir))
                Directory.CreateDirectory(_imagesDir);
        }

        // 給外部取得封面資料夾路徑
        public static string ImagesDir => _imagesDir;

        // ── 讀取 ──────────────────────────────────────────────────
        public static List<Book> LoadBooks()
        {
            var books = new List<Book>();
            if (!File.Exists(_csvPath)) return books;

            using (var sr = new StreamReader(_csvPath, System.Text.Encoding.UTF8))
            {
                string line;
                bool isHeader = true;
                while ((line = sr.ReadLine()) != null)
                {
                    if (isHeader) { isHeader = false; continue; } // 跳過標題列
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var book = Book.FromCsvLine(line);
                    if (book != null) books.Add(book);
                }
            }
            return books;
        }

        // ── 寫入 ──────────────────────────────────────────────────
        public static void SaveBooks(List<Book> books)
        {
            using (var sw = new StreamWriter(_csvPath, false, System.Text.Encoding.UTF8))
            {
                // 標題列
                sw.WriteLine("BookId,Title,Author,Category,Status," +
             "Borrower,DueDate,Rating,AddedDate,Description");

                foreach (var b in books)
                    sw.WriteLine(b.ToCsvLine());
            }
        }

        // ── 自動產生下一個書號（B0001、B0002…） ──────────────────
        public static string GenerateNextId(List<Book> books)
        {
            int max = 0;
            foreach (var b in books)
            {
                if (b.BookId.StartsWith("B") &&
                    int.TryParse(b.BookId.Substring(1), out int n))
                {
                    if (n > max) max = n;
                }
            }
            return "B" + (max + 1).ToString("D4");
        }
    }
}