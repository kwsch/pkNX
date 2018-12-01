namespace pkNX.Game
{
    public static class EditorUtil
    {
        public static string[] SanitizeMoveList(string[] list)
        {
            var movelist = (string[])list.Clone();
            if (movelist.Length < 658)
                return movelist;
            string[] ps = { "P", "S" }; // Distinguish Physical/Special Z Moves
            for (int i = 622; i < 658; i++)
                movelist[i] += $" ({ps[i % 2]})";
            return movelist;
        }
    }
}
