using System;
using System.Linq;
using System.Collections.Generic;

namespace grouping_by_near_dates
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Record> records = new List<Record>();

            // example data
            records.Add(new Record() {
                Id = "0",
                Category = "Cats",
                CreatedDate = new DateTime(2019, 1, 1, 0, 0, 0)
            });
            records.Add(new Record()
            {
                Id = "1",
                Category = "Dogs",
                CreatedDate = new DateTime(2019, 1, 2, 0, 0, 0)
            });
            records.Add(new Record()
            {
                Id = "2",
                Category = "Cats",
                CreatedDate = new DateTime(2019, 1, 2, 0, 0, 15)
            });
            records.Add(new Record()
            {
                Id = "3",
                Category = "Cats",
                CreatedDate = new DateTime(2019, 1, 2, 0, 0, 30)
            });
            records.Add(new Record()
            {
                Id = "4",
                Category = "Cats",
                CreatedDate = new DateTime(2019, 1, 2, 0, 0, 40)
            });
            records.Add(new Record()
            {
                Id = "5",
                Category = "Dogs",
                CreatedDate = new DateTime(2019, 1, 2, 0, 0, 50)
            });
            records.Add(new Record()
            {
                Id = "6",
                Category = "Cows",
                CreatedDate = new DateTime(2019, 1, 1, 0, 0, 0)
            });
            records.Add(new Record()
            {
                Id = "7",
                Category = "Cows",
                CreatedDate = new DateTime(2019, 1, 5, 0, 0, 40)
            });
            records.Add(new Record()
            {
                Id = "8",
                Category = "Cows",
                CreatedDate = new DateTime(2019, 1, 5, 0, 0, 30)
            });
            records.Add(new Record()
            {
                Id = "9",
                Category = "Cows",
                CreatedDate = new DateTime(2019, 1, 8, 0, 0, 40)
            });

            List<Batch> batches = processRecords(records, 60);
            Console.WriteLine("Summary: {0} batches", batches.Count);

            Console.WriteLine("\nDone");
            Console.ReadKey();
        }

        static List<Batch> processRecords(List<Record> records, int thresholdSeconds)
        {
            List<Batch> batches = new List<Batch>();

            var groupedRecords = records.GroupBy(x => x.Category);
            foreach (var batch in groupedRecords)
            {
                Console.WriteLine("Category: {0}, Records: {1}", batch.Key, batch.Count());
                Batch result = processCategory(batch.Key, batch.ToList(), thresholdSeconds);
                if (result != null)
                {
                    batches.Add(result);
                }
            }

            return batches;

        }

        static Batch processCategory(string category, List<Record> records, int thresholdSeconds)
        {
            Batch batch = new Batch
            {
                Category = category,
                Records = new List<Record>()
            };

            records = records.OrderBy(x => x.CreatedDate).Reverse().ToList();
            for (int i = 0; i < records.Count; i++)
            {
                int numRecords = 0;
                double diff = 1;

                for (int j = i; diff <= thresholdSeconds; j++)
                {
                    Console.WriteLine("[{0}] Id: {1}, Diff: {2}", category, records[j].Id, diff);
                    numRecords++;
                    batch.Records.Add(records[j]);

                    if ((j + 1) < records.Count)
                    {
                        diff = (records[j].CreatedDate - records[j + 1].CreatedDate).TotalSeconds;
                    }
                    if (diff > thresholdSeconds 
                        || (j + 1) == records.Count) // handle end of list
                    {
                        // take it
                        Console.WriteLine("[{0}] {1} in batch", category, batch.Records.Count);
                        i = i + numRecords; // jump
                        return batch;
                    }
                }

            }

            return null;
        }

    }
}
