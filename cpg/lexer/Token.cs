﻿using System;

namespace lexer
{
    public class Token<T>
    {

        private static T defTok;

        public static T DefaultToken
        {
            get { return defTok; }
            set { defTok = value; }
        }
        public Token(T token, string value, TokenPosition position)
        {
            TokenID = token;
            Value = value;
            Position = position;
        }

        public Token()
        {
            End = true;
            TokenID = DefaultToken;
        }

        public bool IsEndOfStream
        {
            get
            {
                return TokenID.Equals(DefaultToken);
            }
            private set {
            }
        }

        public TokenPosition Position { get; set; }
        public T TokenID { get; set; }
        public string Value { get; set; }

        public string StringWithoutQuotes
        {
            get
            {
                string result = Value;
                if (result.StartsWith("\""))
                {
                    result = result.Substring(1);
                }
                if (result.EndsWith("\""))
                {
                    result = result.Substring(0, result.Length - 1);
                }
                return result;
            }
            private set { }
        }

        public int IntValue { get {
                return int.Parse(Value);
            } set { } }

        public double DoubleValue
        {
            get
            {
                return double.Parse(Value.Replace(".",","));
            }
            set { }
        }

        public bool End { get; set; }

        public override string ToString()
        {
            return string.Format($" {Position.Index}, {Position.Line}, {Position.Column}");
        }
    }
}
