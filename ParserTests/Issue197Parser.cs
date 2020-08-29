using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using sly.lexer;
using sly.parser.generator;
using sly.parser.parser;
using Xunit.Sdk;

namespace ParserTests
{

    [Lexer(IgnoreWS = true,IgnoreEOL = true)]
    public enum Issue197Token
    {
        [Lexeme(GenericToken.Identifier,IdentifierType.Alpha)]
        ID = 100,
        
        [Lexeme(GenericToken.KeyWord,"first")]
        A = 1, 
        [Lexeme(GenericToken.KeyWord,"endfirst")]
        B = 2,
        [Lexeme(GenericToken.KeyWord,"second")]
        C = 3,
        [Lexeme(GenericToken.KeyWord,"endsecond")]
        D = 4,
        [Lexeme(GenericToken.KeyWord,"E")]
        E = 5,
        [Lexeme(GenericToken.KeyWord,"F")]
        F = 6,
        [Lexeme(GenericToken.KeyWord,"nobody")]
        NOBODY = 42,
        
        [Lexeme(GenericToken.KeyWord,"head")]
        HEAD = 50,
        
        [Lexeme(GenericToken.KeyWord,"endhead")]
        ENDHEAD = 51,
        
        [Lexeme(GenericToken.KeyWord,"rules")]
        RULES = 60,
        
        [Lexeme(GenericToken.KeyWord,"endrules")]
        ENDRULES = 61,
        
    }
    
    public class Issue197Parser
    {
        [Production("rules : head? bodies*")]
        public string Rules(ValueOption<string> head, List<string> body)
        {
            var h = head.Match(
                (x) => x, 
                () => "no body /");
            var hs = h.ToString();
            var b = string.Join("\n\t",body.ToArray<string>());
            b = "h\nbody\n" + b + "\n/body";
            return b;
        }

        [Production("head : HEAD[d] first? second? ENDHEAD[d]")]
        public string head(ValueOption<string> first, ValueOption<string> second)
        {
            var f = first.Match((x) => x, () => "no first/");
            var s = first.Match((x) => x, () => "no second/");
            return $@"head
    {f}
    {s}
/head";
        }
        
        [Production("first : A[d] ID B[d]")]
        public string First(Token<Issue197Token> a)
        {
            return "1st ["+a.Value+"] /1st";
        }
        
        [Production("second : C[d] ID D[d]")]
        public string Second(Token<Issue197Token> a)
        {
            return "2nd ["+a.Value+"] /2nd";
        }

        [Production("bodies : RULES[d] ID+ ENDRULES[d] ")]
        public string Bodies(List<Token<Issue197Token>> body)
        {
            return body.Select((Token<Issue197Token> x) => x.Value)
                .Aggregate(((string x1, string x2) => x1 + "\n" + x2));
        }
        
        
        
    }
}