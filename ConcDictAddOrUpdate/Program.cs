using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcDictAddOrUpdate
{
    class Program
    {
        static ConcurrentDictionary<string, int> _mydictConcu = new ConcurrentDictionary<string, int>();
        static void Main(string[] args)
        {
            string tempGuid = ""; int tempValue = 0; List<int> integerList = Enumerable.Range(0, 10).ToList();
            Parallel.ForEach(integerList, i =>
            {
                tempGuid = Guid.NewGuid().ToString();
                tempValue = i;
                _mydictConcu.TryAdd(tempGuid, tempValue);
            });
            Console.WriteLine($"tempGuid : {tempGuid} tempValue : {tempValue}");
            
            //update
            //here 2nd argument 20 is ignored since tempGuid key exists
            _mydictConcu.AddOrUpdate(tempGuid, 20, (keyAnyNameWorks, oldValueAnyNameWorks) =>
                {
                    return oldValueAnyNameWorks * 2;
                }
            );
            
            // add direct
            _mydictConcu.AddOrUpdate(
                Guid.NewGuid().ToString(),
                211,
                (keyAnyNameWorks, oldValueAnyNameWorks) =>
                {
                    return oldValueAnyNameWorks * 2;
                }
            );
            
            //add using lambda
            _mydictConcu.AddOrUpdate(
                Guid.NewGuid().ToString(), 
                (k1) =>
                {
                    Console.WriteLine($"key for 205 = {k1}");
                    return 205;
                }, 
                (keyAnyNameWorks, oldValueAnyNameWorks) =>
                {
                    return oldValueAnyNameWorks * 2;
                }
            );

            foreach (var item in _mydictConcu)
            {
                Console.WriteLine(item.Key + "-" + item.Value);
            }

            List<int> valuesList = (_mydictConcu.Values).ToList<int>();
            foreach (int item in valuesList)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
