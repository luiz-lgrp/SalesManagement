using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingCRUD.Aplication.Shared
{
    public class MaskCpfPhoneHelper
    {
        public static string FormatCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                return cpf;
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        public static string FormatPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return phone;

            var digits = new string(phone.Where(char.IsDigit).ToArray());

            if (digits.Length == 11)
                return Convert.ToUInt64(digits).ToString(@"\(00\) 0 0000\-0000");
            if (digits.Length == 10)
                return Convert.ToUInt64(digits).ToString(@"\(00\) 0000\-0000");

            return phone;
        }

        public static string OnlyDigits(string? input) =>
        new string(input?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());
    }
}
