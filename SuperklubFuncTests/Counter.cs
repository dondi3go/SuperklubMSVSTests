using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Counter
{
    private int count = 0;
    public int Count
    {
        get { return count; }
    }
    public void Inc() { count++; }
}