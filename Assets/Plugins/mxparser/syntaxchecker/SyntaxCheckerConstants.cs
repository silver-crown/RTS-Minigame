/* Generated By: CSCC: 4.0 (03/25/2012)  Do not edit this line. SyntaxCheckerConstants.cs */
namespace org.mariuszgromada.math.mxparser.syntaxchecker{


using System;
public class SyntaxCheckerConstants {

  public const int EOF = 0;
  public const int WHITESPACE = 5;
  public const int LETTER = 6;
  public const int DIGIT = 7;
  public const int DIGIT_B1 = 8;
  public const int DIGIT_B2 = 9;
  public const int DIGIT_B3 = 10;
  public const int DIGIT_B4 = 11;
  public const int DIGIT_B5 = 12;
  public const int DIGIT_B6 = 13;
  public const int DIGIT_B7 = 14;
  public const int DIGIT_B8 = 15;
  public const int DIGIT_B9 = 16;
  public const int DIGIT_B10 = 17;
  public const int DIGIT_B11 = 18;
  public const int DIGIT_B12 = 19;
  public const int DIGIT_B13 = 20;
  public const int DIGIT_B14 = 21;
  public const int DIGIT_B15 = 22;
  public const int DIGIT_B16 = 23;
  public const int DIGIT_B17 = 24;
  public const int DIGIT_B18 = 25;
  public const int DIGIT_B19 = 26;
  public const int DIGIT_B20 = 27;
  public const int DIGIT_B21 = 28;
  public const int DIGIT_B22 = 29;
  public const int DIGIT_B23 = 30;
  public const int DIGIT_B24 = 31;
  public const int DIGIT_B25 = 32;
  public const int DIGIT_B26 = 33;
  public const int DIGIT_B27 = 34;
  public const int DIGIT_B28 = 35;
  public const int DIGIT_B29 = 36;
  public const int DIGIT_B30 = 37;
  public const int DIGIT_B31 = 38;
  public const int DIGIT_B32 = 39;
  public const int DIGIT_B33 = 40;
  public const int DIGIT_B34 = 41;
  public const int DIGIT_B35 = 42;
  public const int DIGIT_B36 = 43;
  public const int LETTERS = 44;
  public const int INTEGER = 45;
  public const int LEFT_PAR = 46;
  public const int RIGHT_PAR = 47;
  public const int PLUS = 48;
  public const int MINUS = 49;
  public const int MULTIPLY = 50;
  public const int DIV = 51;
  public const int POWER = 52;
  public const int TETRATION = 53;
  public const int MODULO = 54;
  public const int FACTORIAL = 55;
  public const int PERCENTAGE = 56;
  public const int COMMA = 57;
  public const int SEMICOLON = 58;
  public const int EQ = 59;
  public const int UNIT = 60;
  public const int NEQ = 61;
  public const int LT = 62;
  public const int LEQ = 63;
  public const int GT = 64;
  public const int GEQ = 65;
  public const int OR = 66;
  public const int AND = 67;
  public const int NOT = 68;
  public const int BITNOT = 69;
  public const int IMP = 70;
  public const int CIMP = 71;
  public const int NIMP = 72;
  public const int CNIMP = 73;
  public const int NAND = 74;
  public const int EQV = 75;
  public const int NOR = 76;
  public const int BITWISE = 77;
  public const int XOR = 78;
  public const int CHAR = 79;
  public const int DEC_FRACT = 80;
  public const int DEC_FRACT_OR_INT = 81;
  public const int DECIMAL = 82;
  public const int BASE1 = 83;
  public const int BASE2 = 84;
  public const int BASE3 = 85;
  public const int BASE4 = 86;
  public const int BASE5 = 87;
  public const int BASE6 = 88;
  public const int BASE7 = 89;
  public const int BASE8 = 90;
  public const int BASE9 = 91;
  public const int BASE10 = 92;
  public const int BASE11 = 93;
  public const int BASE12 = 94;
  public const int BASE13 = 95;
  public const int BASE14 = 96;
  public const int BASE15 = 97;
  public const int BASE16 = 98;
  public const int BASE17 = 99;
  public const int BASE18 = 100;
  public const int BASE19 = 101;
  public const int BASE20 = 102;
  public const int BASE21 = 103;
  public const int BASE22 = 104;
  public const int BASE23 = 105;
  public const int BASE24 = 106;
  public const int BASE25 = 107;
  public const int BASE26 = 108;
  public const int BASE27 = 109;
  public const int BASE28 = 110;
  public const int BASE29 = 111;
  public const int BASE30 = 112;
  public const int BASE31 = 113;
  public const int BASE32 = 114;
  public const int BASE33 = 115;
  public const int BASE34 = 116;
  public const int BASE35 = 117;
  public const int BASE36 = 118;
  public const int BINARY = 119;
  public const int OCTAL = 120;
  public const int HEXADECIMAL = 121;
  public const int FRACTION = 122;
  public const int IDENTIFIER = 123;
  public const int FUNCTION = 124;
  public const int INVALID_TOKEN = 127;
  public const int UNEXPECTED_CHAR = 128;

  public const int DEFAULT = 0;

  public String[] tokenImage = {
    "<EOF>",
    "\" \"",
    "\"\\t\"",
    "\"\\n\"",
    "\"\\r\"",
    "<WHITESPACE>",
    "<LETTER>",
    "<DIGIT>",
    "\"1\"",
    "<DIGIT_B2>",
    "<DIGIT_B3>",
    "<DIGIT_B4>",
    "<DIGIT_B5>",
    "<DIGIT_B6>",
    "<DIGIT_B7>",
    "<DIGIT_B8>",
    "<DIGIT_B9>",
    "<DIGIT_B10>",
    "<DIGIT_B11>",
    "<DIGIT_B12>",
    "<DIGIT_B13>",
    "<DIGIT_B14>",
    "<DIGIT_B15>",
    "<DIGIT_B16>",
    "<DIGIT_B17>",
    "<DIGIT_B18>",
    "<DIGIT_B19>",
    "<DIGIT_B20>",
    "<DIGIT_B21>",
    "<DIGIT_B22>",
    "<DIGIT_B23>",
    "<DIGIT_B24>",
    "<DIGIT_B25>",
    "<DIGIT_B26>",
    "<DIGIT_B27>",
    "<DIGIT_B28>",
    "<DIGIT_B29>",
    "<DIGIT_B30>",
    "<DIGIT_B31>",
    "<DIGIT_B32>",
    "<DIGIT_B33>",
    "<DIGIT_B34>",
    "<DIGIT_B35>",
    "<DIGIT_B36>",
    "<LETTERS>",
    "<INTEGER>",
    "\"(\"",
    "\")\"",
    "\"+\"",
    "\"-\"",
    "\"*\"",
    "\"/\"",
    "\"^\"",
    "\"^^\"",
    "\"#\"",
    "\"!\"",
    "\"%\"",
    "\",\"",
    "\";\"",
    "<EQ>",
    "<UNIT>",
    "<NEQ>",
    "\"<\"",
    "\"<=\"",
    "\">\"",
    "\">=\"",
    "<OR>",
    "<AND>",
    "\"~\"",
    "\"@~\"",
    "\"-->\"",
    "\"<--\"",
    "\"-/>\"",
    "\"</-\"",
    "<NAND>",
    "\"<->\"",
    "<NOR>",
    "<BITWISE>",
    "\"(+)\"",
    "<CHAR>",
    "<DEC_FRACT>",
    "<DEC_FRACT_OR_INT>",
    "<DECIMAL>",
    "<BASE1>",
    "<BASE2>",
    "<BASE3>",
    "<BASE4>",
    "<BASE5>",
    "<BASE6>",
    "<BASE7>",
    "<BASE8>",
    "<BASE9>",
    "<BASE10>",
    "<BASE11>",
    "<BASE12>",
    "<BASE13>",
    "<BASE14>",
    "<BASE15>",
    "<BASE16>",
    "<BASE17>",
    "<BASE18>",
    "<BASE19>",
    "<BASE20>",
    "<BASE21>",
    "<BASE22>",
    "<BASE23>",
    "<BASE24>",
    "<BASE25>",
    "<BASE26>",
    "<BASE27>",
    "<BASE28>",
    "<BASE29>",
    "<BASE30>",
    "<BASE31>",
    "<BASE32>",
    "<BASE33>",
    "<BASE34>",
    "<BASE35>",
    "<BASE36>",
    "<BINARY>",
    "<OCTAL>",
    "<HEXADECIMAL>",
    "<FRACTION>",
    "<IDENTIFIER>",
    "<FUNCTION>",
    "\"[\"",
    "\"]\"",
    "<INVALID_TOKEN>",
    "<UNEXPECTED_CHAR>",
  };

}
}
