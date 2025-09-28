using Quasar.Server.Models;
using System.Collections;
using System.Windows.Forms;

namespace Quasar.Server.Utilities
{
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// 指定要排序的列
        /// </summary>
        private int _columnToSort;

        /// <summary>
        /// 指定排序的顺序（即'升序'）。
        /// </summary>
        private SortOrder _orderOfSort;

        /// <summary>
        /// 不区分大小写的比较器对象
        /// </summary>
        private readonly CaseInsensitiveComparer _objectCompare;

        /// <summary>
        /// 指定是否需要数字或文本比较
        /// </summary>
        private bool _needNumberCompare;

        /// <summary>
        /// 类构造函数。初始化各种元素
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            _columnToSort = 0;

            // Initialize the sort order to 'none'
            _orderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            _objectCompare = new CaseInsensitiveComparer();

            _needNumberCompare = false;
        }

        /// <summary>
        /// 此方法继承自IComparer接口。它使用不区分大小写的比较来比较传入的两个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象</param>
        /// <param name="y">要比较的第二个对象</param>
        /// <returns>比较结果。如果相等则为"0"，如果'x'小于'y'则为负数，如果'x'大于'y'则为正数</returns>
        public int Compare(object x, object y)
        {
            // Cast the objects to be compared to ListViewItem objects
            var listviewX = (ListViewItem) x;
            var listviewY = (ListViewItem) y;

            if (listviewX.SubItems[0].Text == ".." || listviewY.SubItems[0].Text == "..")
                return 0;

            // Compare the two items
            int compareResult;

            if (_needNumberCompare)
            {
                long a, b;

                if (listviewX.Tag is FileManagerListTag)
                {
                    // fileSize to be compared
                    a = (listviewX.Tag as FileManagerListTag).FileSize;
                    b = (listviewY.Tag as FileManagerListTag).FileSize;
                    compareResult = a >= b ? (a == b ? 0 : 1) : -1;

                }
                else
                {
                    if (long.TryParse(listviewX.SubItems[_columnToSort].Text, out a)
                        && long.TryParse(listviewY.SubItems[_columnToSort].Text, out b))
                    {
                        compareResult = a >= b ? (a == b ? 0 : 1) : -1;
                    }
                    else
                    {
                        compareResult = _objectCompare.Compare(listviewX.SubItems[_columnToSort].Text,
                     listviewY.SubItems[_columnToSort].Text);
                    }
                }
            }
            else
            {
                compareResult = _objectCompare.Compare(listviewX.SubItems[_columnToSort].Text,
                    listviewY.SubItems[_columnToSort].Text);
            }

            // Calculate correct return value based on object comparison
            if (_orderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (_orderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// 获取或设置应用排序操作的列号（默认为'0'）。
        /// </summary>
        public int SortColumn
        {
            set { _columnToSort = value; }
            get { return _columnToSort; }
        }

        /// <summary>
        /// 获取或设置要应用的排序顺序（例如，'升序'或'降序'）。
        /// </summary>
        public SortOrder Order
        {
            set { _orderOfSort = value; }
            get { return _orderOfSort; }
        }

        /// <summary>
        /// 指定是否需要数字或文本比较。
        /// </summary>
        public bool NeedNumberCompare
        {
            set { _needNumberCompare = value; }
            get { return _needNumberCompare; }
        }
    }
}