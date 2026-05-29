using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using calc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Calc
{
    enum AnswerMode
    {
        Round = 0,
        Ceiling = 1,
        Floor = 2,
        Truncate = 3,
        Float = 4
    }

    class Calculate : MonoBehaviour


    {
        static float mathfunc(string name, List<float> arg)
        {
            if (string.Equals(name, "pin", StringComparison.OrdinalIgnoreCase)) return (float) Math.PI;
            if (string.Equals(name, "sin", StringComparison.OrdinalIgnoreCase)) return (float) Math.Sin(arg[0]);
            if (string.Equals(name, "cos", StringComparison.OrdinalIgnoreCase)) return (float) Math.Cos(arg[0]);
            if (string.Equals(name, "tan", StringComparison.OrdinalIgnoreCase)) return (float) Math.Tan(arg[0]);
            if (string.Equals(name, "asin", StringComparison.OrdinalIgnoreCase)) return (float) Math.Asin(arg[0]);
            if (string.Equals(name, "acos", StringComparison.OrdinalIgnoreCase)) return (float) Math.Acos(arg[0]);
            if (string.Equals(name, "atan", StringComparison.OrdinalIgnoreCase)) return (float) Math.Atan(arg[0]);

            if (string.Equals(name, "max", StringComparison.OrdinalIgnoreCase)) return Math.Max(arg[0], arg[1]);
            if (string.Equals(name, "min", StringComparison.OrdinalIgnoreCase)) return Math.Min(arg[0], arg[1]);

            if (string.Equals(name, "log", StringComparison.OrdinalIgnoreCase))
                return (float) Math.Log(arg[0], arg[1]); //対数
            if (string.Equals(name, "log10", StringComparison.OrdinalIgnoreCase))
                return (float) Math.Log10(arg[0]); //底１０

            if (string.Equals(name, "abs", StringComparison.OrdinalIgnoreCase)) return Math.Abs(arg[0]); //絶対値
            if (string.Equals(name, "sqrt", StringComparison.OrdinalIgnoreCase)) return (float) Math.Sqrt(arg[0]); //平方根
            if (string.Equals(name, "pow", StringComparison.OrdinalIgnoreCase))
                return (float) Math.Pow(arg[0], arg[1]); //乗数
            if (string.Equals(name, "sign", StringComparison.OrdinalIgnoreCase)) return Math.Sign(arg[0]); //符号を取得

            if (string.Equals(name, "round", StringComparison.OrdinalIgnoreCase))
                return (float) Math.Round(arg[0]); //四捨五入
            if (string.Equals(name, "ceiling", StringComparison.OrdinalIgnoreCase))
                return (float) Math.Ceiling(arg[0]); //切り上げ
            if (string.Equals(name, "floor", StringComparison.OrdinalIgnoreCase))
                return (float) Math.Floor(arg[0]); //切り捨て 正負考慮（通常）
            if (string.Equals(name, "truncate", StringComparison.OrdinalIgnoreCase))
                return (float) Math.Truncate(arg[0]); //切り捨て 正負非考慮


            if (string.Equals(name, "random", StringComparison.OrdinalIgnoreCase))
                return UnityEngine.Random.Range(arg[0], arg[1]);
            return 0;
        }

        static String DiceRole(string diceRole)
        {
            StringBuilder sb = new StringBuilder(diceRole.Length);
            sb.Append(diceRole);


            MatchCollection matches = Regex.Matches(diceRole, "\\d+D\\d+", RegexOptions.IgnoreCase);
            int margin = 0;
            foreach (Match match in matches)
            {
                var x = new DiceExpression(match.Value.ToLower(), DiceExpressionOptions.SimplifyStringValue);
                string eval = x.Evaluate().ToString();
                sb.Replace(match.Value, eval, match.Index - margin, match.Length);
                margin = margin + match.Length - eval.Length;
            }

            diceRole = sb.ToString();
            return diceRole;
        }


        static String ChangePow(string text)
        {
            StringBuilder sb = new StringBuilder(text.Length);
            sb.Append(text);


            MatchCollection matches = Regex.Matches(text, "(\\d+)\\^(\\d+)", RegexOptions.IgnoreCase);
            int margin = 0;
            foreach (Match match in matches)
            {
                string eval = Math.Pow(Convert.ToDouble(match.Groups[1].Value), Convert.ToDouble(match.Groups[2].Value))
                    .ToString();
                sb.Replace(match.Value, eval, match.Index - margin, match.Length);
                margin = margin + match.Length - eval.Length;
            }

            text = sb.ToString();
            return text;
        }

        [Button]
        public float Evaluate(String formulaText, Dictionary<string, string> argDictionary = null,
            AnswerMode mode = AnswerMode.Round)
        {
            try
            {
                float answer = SubEvaluate(formulaText, argDictionary);


                switch (mode)
                {
                    // Round = 0,Ceiling = 1,Floor = 2,Truncate = 3, Float=4foraa
                    case AnswerMode.Round:
                        return (float) Math.Round(answer);
                    case AnswerMode.Ceiling:
                        return (float) Math.Ceiling(answer);
                    case AnswerMode.Floor:
                        return (float) Math.Floor(answer);
                    case AnswerMode.Truncate:
                        return (float) Math.Truncate(answer);
                    case AnswerMode.Float:
                        return answer;
                    default:
                        return answer;
                }
            }
            catch (System.Exception)
            {
                Debug.Log("Caluculation Error");
                return 0;
            }
        }

        private float SubEvaluate(string formulaText, Dictionary<string, string> argDictionary = null)
        {
            float answer;
            if (formulaText.Length != 0)
            {
                if (Regex.Match(formulaText, "\\d+D\\d+", RegexOptions.IgnoreCase).Success)
                    formulaText = DiceRole(formulaText);

                if (Regex.Match(formulaText, "\\d+\\^\\d+", RegexOptions.IgnoreCase).Success)
                    formulaText = ChangePow(formulaText);

                var formura = Calc.Analyze(formulaText);


                Func<string, List<float>, float> f = (name, arg) =>
                {
                    if (argDictionary != null)
                    {
                        if (argDictionary.ContainsKey(name.ToString()))
                        {
                            return SubEvaluate(argDictionary[name.ToString()]);
                        }
                    }

                    return mathfunc(name, arg);
                };


                answer = formura.Calc(f);
            }
            else
            {
                answer = 0;
            }

            return answer;
        }
    }
}