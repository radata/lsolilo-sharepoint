using System.Text;
using System.Text.RegularExpressions;

namespace FPS.Core
{
    public static class NumbersAsTextInPolish
    {
        #region Fields

        private static string zero = "zero";
        private static string[] digits = { string.Empty, " jeden ", " dwa ", " trzy ", " cztery ", " pięć ", " sześć ", " siedem ", " osiem ", " dziewięć " };
        private static string[] tens = { string.Empty, " dziesięć ", " dwadzieścia ", " trzydzieści ", " czterdzieści ", " pięćdziesiąt ", " sześćdziesiąt ", " siedemdziesiąt ", " osiemdziesiąt ", " dziewięćdziesiąt " };
        private static string[] teens = { "dziesięć", " jedenaście ", " dwanaście ", " trzynaście ", " czternaście ", " piętnaście ", " szesnaście ", " siedemnaście ", " osiemnaście ", " dziewiętnaście " };
        private static string[] hundreds = { string.Empty, " sto ", " dwieście ", " trzysta ", " czterysta ", " pięćset ", " sześćset ", " siedemset ", " osiemset ", " dziewięćset " };
        private static string[] thousandsSingular = { string.Empty, " tysiąc ", " tysiące ", " tysiące ", " tysiące ", " tysięcy ", " tysięcy ", " tysięcy ", " tysięcy ", " tysięcy " };
        private static string[] thousandsPlural = { " tysięcy ", " tysięcy ", " tysiące ", " tysiące ", " tysiące ", " tysięcy ", " tysięcy ", " tysięcy ", " tysięcy ", " tysięcy " };

        private static string[] currency = { " złoty ", " złote ", " złotych" };

        #endregion

        #region Methods

        /// <summary>
        /// Gets the numbers as text.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>Numbers as text.</returns>
        public static string GetNumbersAsText(decimal number)
        {
            StringBuilder numbersAsText = new StringBuilder();

            // 0-999
            int value = (int)number;

            if (value == 0)
                return zero;

            int oneDigit = value % 10;
            int twoDigits = value % 100;
            int threeDigits = (value % 1000) / 100;

            if (twoDigits > 10 && twoDigits < 20)
            {
                numbersAsText.Insert(0, teens[oneDigit]);
            }
            else
            {
                numbersAsText.Insert(0, digits[oneDigit]);
                numbersAsText.Insert(0, tens[twoDigits / 10]);
            }

            numbersAsText.Insert(0, hundreds[threeDigits]);

            // 1000-999999
            value = value / 1000;
            oneDigit = value % 10;
            twoDigits = value % 100;
            threeDigits = (value % 1000) / 100;

            if ((value % 1000) / 10 == 0)
            {
                numbersAsText.Insert(0, thousandsSingular[oneDigit]);
                if (oneDigit > 1)
                    numbersAsText.Insert(0, digits[oneDigit]);

                return numbersAsText.ToString().RemoveMultipleSpaces(); 
            }

            if (twoDigits >= 10 && twoDigits < 20)
            {
                numbersAsText.Insert(0, thousandsPlural[0]);
                numbersAsText.Insert(0, teens[twoDigits % 10]);
            }
            else
            {
                numbersAsText.Insert(0, thousandsPlural[oneDigit]);
                numbersAsText.Insert(0, digits[oneDigit]);
                numbersAsText.Insert(0, tens[twoDigits / 10]);
            }

            numbersAsText.Insert(0, hundreds[threeDigits]);
            return numbersAsText.ToString().RemoveMultipleSpaces();
        }

        /// <summary>
        /// Gets the amount as text.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>Amount as text.</returns>
        public static string GetAmountAsText(decimal number)
        {
            var result = GetNumbersAsText(number);

            int value = (int)number;
            int oneDigit = value % 10;
            int twoDigits = value % 100;

            // defalut as "złotych"
            int currencyIndex = 2;

            if (twoDigits == 1)
            {
                currencyIndex = 0;
            }
            else if (oneDigit > 1 && oneDigit < 5 && !(twoDigits > 10 && twoDigits < 20))
            {
                currencyIndex = 1;
            }

            int afterComma = (int)(number * 100) % 100;
            result += string.Format(" {0} {1:00}/100", currency[currencyIndex], afterComma);
            return result.RemoveMultipleSpaces();
        }

        #endregion
    }
}
