using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utils
{
    public static class StringExtension
    {
        /// <summary>
        /// 全角字符转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDbc(string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char) 32;
                    continue;
                }

                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char) (c[i] - 65248);
            }

            return new string(c);
        }


        /// <summary>
        /// 半角字符转全角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSbc(string input)
        {
            // 半角转全角：
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char) 12288;
                    continue;
                }

                if (c[i] < 127)
                    c[i] = (char) (c[i] + 65248);
            }

            return new string(c);
        }

        /// <summary>
        /// 字符串JObject种，添加key value
        /// </summary>
        /// <param name="objectJson"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string JsonStringAddProperty(string objectJson, string key, object value)
        {
            var obj = JObject.Parse(objectJson);
            obj.Add(new JProperty(key, value));
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 带单引号的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSqlValue(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "''";
            }

            var sqlValue = value.Trim().Replace("'", "''");
            return $"'{sqlValue}'";
        }

        /// <summary>
        /// 电话号码混淆
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static string ConfusePhoneNumber(this string phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length > 7)
            {
                phoneNumber = $"{phoneNumber.Substring(0, 3)}****{phoneNumber.Substring(7)}";
            }

            return phoneNumber;
        }

        /// <summary>
        /// 姓名-混淆： 李*；李*柒
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ConfuseName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            if (name.Length == 1)
            {
                return name;
            }
            else
            {
                var part1 = name.Substring(0, 1);
                var part2 = "*";
                var part3 = name.Substring(2, name.Length - 2);
                return $"{part1}{part2}{part3}";
            }
        }

        /// <summary>
        /// email-混淆
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string ConfuseEmail(this string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return string.Empty;
                }

                email = email.Trim();
                var lastIndex = email.LastIndexOf('@');
                var firstIndex = email.IndexOf('@');
                if (lastIndex != firstIndex || lastIndex == 0 || lastIndex == -1)
                {
                    return email;
                }

                if (email.Length == 1)
                {
                    return email;
                }
                else
                {
                    var newSubstring = email.Substring(0, firstIndex);
                    var lastString = email.Substring(firstIndex, email.Length - firstIndex);
                    switch (newSubstring.Length)
                    {
                        case 1:
                            return $"*{lastString}";
                        case 2:
                            return $"{newSubstring.Substring(0, 1)}*{lastString}";
                        default:
                        {
                            var num = newSubstring.Length / 3;
                            var part1 = newSubstring.Substring(0, num);
                            var part2 = string.Empty;
                            for (var i = 0; i < newSubstring.Length - num - num; i++)
                            {
                                part2 += "*";
                            }

                            var part3 = newSubstring.Substring(newSubstring.Length - num, num);
                            return $"{part1}{part2}{part3}{lastString}";
                        }
                    }
                }
            }
            catch (Exception)
            {
                return email;
            }
        }


        /// <summary>
        /// 字符串转换为guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="AggregateException"></exception>
        public static Guid StringToGuid(this string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ApplicationException("input string 'id' is null or empty");
            }

            try
            {
                return Guid.Parse(id);
            }
            catch (Exception)
            {
                throw new AggregateException("无效的guid字符串强转为Guid类型错误，请核对！");
            }
        }
    }
}