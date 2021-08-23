using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;

namespace Coop.Web.PaysParser
{
    public class PaysParser : IPaysParser
    {
        public const string NUMBER_COLUMN = "Номер";
        public const string NAME_COLUMN = "ФИО";
        public const string DATE_COLUMN = "Дата";
        public const string AMOUNT_COLUMN = "Сумма";

        public List<PayRecord> ParseFromStream(MemoryStream stream)
        {
            var results = new List<PayRecord>();
            int numberId = -1;
            int nameId = -1;
            int dateId = -1;
            int amountId = -1;

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                reader.Read();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    switch (reader.GetString(i))
                    {
                        case NUMBER_COLUMN:
                            numberId = i;
                            break;
                        case NAME_COLUMN:
                            nameId = i;
                            break;
                        case DATE_COLUMN:
                            dateId = i;
                            break;
                        case AMOUNT_COLUMN:
                            amountId = i;
                            break;
                    }
                }

                if (numberId == -1 || nameId == -1 || dateId == -1 || amountId == -1)
                {
                    throw new ArgumentException($"Не найден один из столбцов: {NUMBER_COLUMN}, {NAME_COLUMN}, {DATE_COLUMN}, {AMOUNT_COLUMN}");
                }
                
                while (reader.Read())
                {
                    if (!decimal.TryParse(reader.GetValue(amountId)?.ToString(), out var amount)) continue;
                    var number = reader.GetValue(numberId)?.ToString();
                    if (string.IsNullOrWhiteSpace(number)) continue;
                    if (!DateTime.TryParse(reader.GetValue(dateId)?.ToString(), out var date)) continue;
                    var fio = reader.GetValue(nameId)?.ToString();
                    if (string.IsNullOrWhiteSpace(number)) continue;
                    
                    var rec = new PayRecord()
                    {
                        InventoryNumber = number,
                        Amount = amount,
                        PayeerName = fio,
                        PayTimestamp = date
                    };
                    
                    results.Add(rec);
                }
            }

            return results;
        }
    }
}