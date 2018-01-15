namespace Com.EnjoyCodes.DapperRepositories.Utilities
{
    public class SqlInject
    {
        public bool Validate(string s)
        {
            //var words = "'|and|exec|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare|;|or|-|+|,";
            var words = "'|exec|insert|select|delete|update|*|%|chr|mid|truncate|char|declare|;|or|-|+|=|,";

            var word = words.Split('|');

            for (int i = 0; i < word.Length; i++)
            {
                if (s.IndexOf(word[i]) >= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
