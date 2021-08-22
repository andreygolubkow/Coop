using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;

namespace Coop.Web.DebtsParser
{
    public class DebtParser: IDebtParser
    {
        public const string NUMBER_COLUMN = "Номер";
        public const string DEBT_COLUMN = "Долг";
        
        public List<DebtRecord> ParseFromStream(MemoryStream stream)
        {
            var list = new List<DebtRecord>();
            int numColumn = -1;
            int debtColumn = -1;
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                reader.Read();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetString(i) == NUMBER_COLUMN)
                    {
                        numColumn = i;
                    }

                    if (reader.GetString(i) == DEBT_COLUMN)
                    {
                        debtColumn = i;
                    }
                }

                if (numColumn == -1 || debtColumn == -1)
                {
                    throw new ArgumentException($"Не найден один из столбцов:{DEBT_COLUMN},{NUMBER_COLUMN}");
                }
                while (reader.Read())
                {
                    if (!decimal.TryParse(reader.GetValue(debtColumn)?.ToString(), out var amount)) continue;
                    var rec = new DebtRecord()
                              {
                                  InventoryNumber = reader.GetValue(numColumn)?.ToString(),
                                  Amount = amount
                              };
                        
                    if (string.IsNullOrWhiteSpace(rec.InventoryNumber)) continue;
                    list.Add(rec);
                }
            }

            return list;
        }
    }
}