using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSUDental.Models
{
    public enum SortDirection
	{
        asc,
        desc
	}

    public class PageDetails
    {
        public PageDetails()
        {
            page = 1;
            pageSize = 50;
            sortColumn = "";
            direction = SortDirection.asc;
        }
        public PageDetails(int page, int pageSize, String sortColumn, String direction)
        {
            this.page = page;
            this.pageSize = pageSize;
            this.sortColumn = sortColumn;
            this.direction = direction.ToLower().Equals("asc")?SortDirection.asc:SortDirection.desc;
        }
        
        public int page { get; set; }
        public int pageSize {get;set;}
        public String sortColumn { get;set;}
        public SortDirection direction {get;set;}

        public int GetStartingRow()
        {
            return (page - 1) * pageSize + 1;
        }
        public int GetEndingRow()
        {
            return (page) * pageSize;
        }
        public int GetPageSize()
        {
            return Math.Max(Math.Min(1, pageSize),200);
        }
    }
}