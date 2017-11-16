using System.Collections.Generic;

namespace ProjetSynthese
{
    public static class ListExtension
    {
        public static T GetLastIndex<T>(List<T> list)
        {
            return list[list.Count - 1];
        }
    }
}