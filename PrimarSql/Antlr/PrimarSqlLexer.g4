lexer grammar PrimarSqlLexer;

channels { PRIMARSQLCOMMENT, ERRORCHANNEL }

// SKIP

SPACE:                               [ \t\r\n]+    -> channel(HIDDEN);
SPEC_MYSQL_COMMENT:                  '/*!' .+? '*/' -> channel(PRIMARSQLCOMMENT);
COMMENT_INPUT:                       '/*' .*? '*/' -> channel(HIDDEN);
LINE_COMMENT:                        (
                                       ('-- ' | '#') ~[\r\n]* ('\r'? '\n' | EOF)
                                       | '--' ('\r'? '\n' | EOF)
                                     ) -> channel(HIDDEN);

SELECT:                              'SELECT';
STRONGLY:                            'STRONGLY';
EVENTUALLY:                          'EVENTUALLY';
AS:                                  'AS';
FROM:                                'FROM';
WHERE:                               'WHERE';
GROUP:                               'GROUP';
BY:                                  'BY';
WITH:                                'WITH';
LIMIT:                               'LIMIT';
LIMITS:                              'LIMITS';
OFFSET:                              'OFFSET';
TRUE:                                'TRUE';
FALSE:                               'FALSE';
VARCHAR:                             'VARCHAR';
TEXT:                                'TEXT';
MEDIUMTEXT:                          'MEDIUMTEXT';
LONGTEXT:                            'LONGTEXT';
STRING:                              'STRING';
INT:                                 'INT';
INTEGER:                             'INTEGER';
BIGINT:                              'BIGINT';
BOOL:                                'BOOL';
BOOLEAN:                             'BOOLEAN';
LIST:                                'LIST';
BINARY:                              'BINARY';
NUMBER_LIST:                         'NUMBER_LIST';
STRING_LIST:                         'STRING_LIST';
BINARY_LIST:                         'BINARY_LIST';
ORDER:                               'ORDER';
CREATE:                              'CREATE';
INDEX:                               'INDEX';
INDEXES:                             'INDEXES';
ON:                                  'ON';
LOCAL:                               'LOCAL';
GLOBAL:                              'GLOBAL';
ALL:                                 'ALL';
KEYS:                                'KEYS';
ONLY:                                'ONLY';
INCLUDE:                             'INCLUDE';
TABLE:                               'TABLE';
TABLES:                              'TABLES';
HASH:                                'HASH';
KEY:                                 'KEY';
RANGE:                               'RANGE';
THROUGHPUT:                          'THROUGHPUT';
BILLINGMODE:                         'BILLINGMODE';
PROVISIONED:                         'PROVISIONED';
PAY_PER_REQUEST:                     'PAY_PER_REQUEST';
ON_DEMAND:                           'ON_DEMAND';
ALTER:                               'ALTER';
ADD:                                 'ADD';
DROP:                                'DROP';
INSERT:                              'INSERT';
IGNORE:                              'IGNORE';
INTO:                                'INTO';
VALUES:                              'VALUES';
VALUE:                               'VALUE';
ASC:                                 'ASC';
DESC:                                'DESC';
DESCRIBE:                            'DESCRIBE';
NOT:                                 'NOT';
IF_NOT_EXISTS:                       'IF_NOT_EXISTS';
ATTRIBUTE_EXISTS:                    'ATTRIBUTE_EXISTS';
ATTRIBUTE_NOT_EXISTS:                'ATTRIBUTE_NOT_EXISTS';
ATTRIBUTE_TYPE:                      'ATTRIBUTE_TYPE';
BEGINS_WITH:                         'BEGINS_WITH';
CONTAINS:                            'CONTAINS';
SIZE:                                'SIZE';
IF:                                  'IF';
EXISTS:                              'EXISTS';
DEFAULT:                             'DEFAULT';
BETWEEN:                             'BETWEEN';
AND:                                 'AND';
LIKE:                                'LIKE';
REGEXP:                              'REGEXP';
RLIKE:                               'RLIKE';
IN:                                  'IN';
IS:                                  'IS';
SOME:                                'SOME';
ESCAPE:                              'ESCAPE';
ROW:                                 'ROW';
XOR:                                 'XOR';
OR:                                  'OR';
START:                               'START';
ENDPOINTS:                           'ENDPOINTS';
SHOW:                                'SHOW';
UPDATE:                              'UPDATE';
SET:                                 'SET';
DELETE:                              'DELETE';
PARTITION:                           'PARTITION';
SORT:                                'SORT';
CURRENT_DATE:                        'CURRENT_DATE';
CURRENT_TIME:                        'CURRENT_TIME';
CURRENT_TIMESTAMP:                   'CURRENT_TIMESTAMP';
CAST:                                'CAST';
SUBSTR:                              'SUBSTR';
SUBSTRING:                           'SUBSTRING';
TRIM:                                'TRIM';
BOTH:                                'BOTH';
LEADING:                             'LEADING';
TRAILING:                            'TRAILING';
FOR:                                 'FOR';

NULL_LITERAL:                        'NULL';

// Operators. Arithmetics

STAR:                                '*';
DIVIDE:                              '/';
MODULE:                              '%';
PLUS:                                '+';
MINUSMINUS:                          '--';
MINUS:                               '-';
DIV:                                 'DIV';
MOD:                                 'MOD';

// Operators. Comparation

EQUAL_SYMBOL:                        '=';
GREATER_SYMBOL:                      '>';
LESS_SYMBOL:                         '<';
EXCLAMATION_SYMBOL:                  '!';


// Operators. Bit

BIT_NOT_OP:                          '~';
BIT_OR_OP:                           '|';
BIT_AND_OP:                          '&';
BIT_XOR_OP:                          '^';

// Constructors symbols

DOT:                                 '.';
LR_BRACKET:                          '(';
RR_BRACKET:                          ')';
COMMA:                               ',';
SEMI:                                ';';
AT_SIGN:                             '@';
ZERO_DECIMAL:                        '0';
ONE_DECIMAL:                         '1';
TWO_DECIMAL:                         '2';
SINGLE_QUOTE_SYMB:                   '\'';
DOUBLE_QUOTE_SYMB:                   '"';
REVERSE_QUOTE_SYMB:                  '`';
COLON_SYMB:                          ':';

// Literal Primitives

STRING_LITERAL:                      DQUOTA_STRING | SQUOTA_STRING; //  | BQUOTA_STRING
DECIMAL_LITERAL:                     DEC_DIGIT+;
HEXADECIMAL_LITERAL:                 'X' '\'' (HEX_DIGIT HEX_DIGIT)+ '\''
                                     | '0X' HEX_DIGIT+;

REAL_LITERAL:                        (DEC_DIGIT+)? '.' DEC_DIGIT+
                                     | DEC_DIGIT+ '.' EXPONENT_NUM_PART
                                     | (DEC_DIGIT+)? '.' (DEC_DIGIT+ EXPONENT_NUM_PART)
                                     | DEC_DIGIT+ EXPONENT_NUM_PART;
BIT_STRING:                          BIT_STRING_L;

// Identifiers

ID:                                  ID_LITERAL;
DOUBLE_QUOTE_ID:                     '"' ~'"'+ '"';
REVERSE_QUOTE_ID:                    '`' ~'`'+ '`';


// Fragments for Literal primitives

fragment EXPONENT_NUM_PART:          'E' [-+]? DEC_DIGIT+;
fragment ID_LITERAL:                 [A-Z_$0-9]*?[A-Z_$]+?[A-Z_$0-9]*;
fragment DQUOTA_STRING:              '"' ( '\\'. | '""' | ~('"'| '\\') )* '"';
fragment SQUOTA_STRING:              '\'' ('\\'. | '\'\'' | ~('\'' | '\\'))* '\'';
fragment BQUOTA_STRING:              '`' ( '\\'. | '``' | ~('`'|'\\'))* '`';
fragment HEX_DIGIT:                  [0-9A-F];
fragment DEC_DIGIT:                  [0-9];
fragment BIT_STRING_L:               'B' '\'' [01]+ '\'';

// Last tokens must generate Errors

ERROR_RECONGNIGION:                  .    -> channel(ERRORCHANNEL);
