using System;
using System.Collections.Generic;

namespace FoodMarketDMS.Core.Extensions
{
    public static class ListExtension
    {
        public static void TrimString(this List<string> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            for (int i = 0, l = list.Count; i < l; i++)
            {
                list[i] = list[i].Trim();
            }
        }

        public static void RemoveEmptyString(this List<string> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            for (int i = 0, l = list.Count; i < l; i++)
            {
                string item = list[i];
                if (string.IsNullOrWhiteSpace(item))
                {
                    list.RemoveAt(i--);
                    l--;
                }
            }
        }
    }
}
