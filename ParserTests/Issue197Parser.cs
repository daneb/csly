using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using sly.lexer;
using sly.parser.generator;
using sly.parser.parser;
using Xunit.Sdk;

namespace ParserTests
{

    [Lexer(IgnoreWS = true,IgnoreEOL = true,KeyWordIgnoreCase = false)]
    public enum Issue197Token
    {
        [Lexeme(GenericToken.Identifier,IdentifierType.Alpha)]
        ID = 100,
        
        [Lexeme(GenericToken.KeyWord,"first")]
        FIRST = 1, 
        [Lexeme(GenericToken.KeyWord,"endfirst")]
        ENDFIRST = 2,
        [Lexeme(GenericToken.KeyWord,"second")]
        SECOND = 3,
        [Lexeme(GenericToken.KeyWord,"endsecond")]
        ENDSECOND = 4,
        [Lexeme(GenericToken.KeyWord,"nobody")]
        NOBODY = 42,
        
        [Lexeme(GenericToken.KeyWord,"head")]
        HEAD = 50,
        
        [Lexeme(GenericToken.KeyWord,"endhead")]
        ENDHEAD = 51,
        
        [Lexeme(GenericToken.KeyWord,"rule")]
        RULE = 60,
        
        [Lexeme(GenericToken.KeyWord,"endrule")]
        ENDRULE = 61,
        
    }
    
    public class Issue197Parser
    {
        [Production("rules : head? rule*")]
        public string Rules(ValueOption<string> head, List<string> body)
        {
            var h = head.Match(
                (x) => x, 
                () => "no body /");
            var hs = h.ToString();
            var b = string.Join("\n\t",body.ToArray<string>());
            b = "h\nbody\n" + b + "\n/body";
            return hs+b;
        }

        [Production("head : HEAD[d] first? second? ENDHEAD[d]")]
        public string head(ValueOption<string> first, ValueOption<string> second)
        {
            var f = first.Match((x) => x, () => "no first/");
            var s = second.Match((x) => x, () => "no second/");
            return $@"head
    {f}
    {s}
/head";
        }
        
        [Production("first : FIRST[d] ID ENDFIRST[d]")]
        public string First(Token<Issue197Token> a)
        {
            return "1st ["+a.Value+"] /1st";
        }
        
        [Production("second : SECOND[d] ID ENDSECOND[d]")]
        public string Second(Token<Issue197Token> a)
        {
            return "2nd ["+a.Value+"] /2nd";
        }

        [Production("rule : RULE[d] ID+ ENDRULE[d] ")]
        public string Rule(List<Token<Issue197Token>> body)
        {
            return body.Select((Token<Issue197Token> x) => x.Value)
                .Aggregate(((string x1, string x2) => x1 + "\n" + x2));
        }
        
        
        
    }
}