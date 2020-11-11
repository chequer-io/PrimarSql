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

// Keywords
// Common Keywords
CREATE:                              'CREATE';
TABLE:                               'TABLE';
ALTER:     'ALTER';
KEY:       'KEY';
RANGE:     'RANGE';
DROP:      'DROP';
INDEX:     'INDEX';
ON:        'ON';
INSERT:    'INSERT';
INTO:      'INTO';
PARTITION: 'PARTITION';
VALUES:    'VALUES';
DELETE:    'DELETE';
FROM:      'FROM';
WHERE:     'WHERE';
LIMIT:     'LIMIT';
AS:        'AS';
UPDATE:    'UPDATE';
SET:       'SET';
ORDER:     'ORDER';
USING:     'USING';
ASC:       'ASC';
DESC:      'DESC';
SELECT:    'SELECT';
SHOW:      'SHOW';
IN:        'IN';
KEYS:      'KEYS';
TRUE:      'TRUE';
FALSE:     'FALSE';
NULL_LITERAL:                        'NULL';
NOT:                                 'NOT';
DEFAULT:                             'DEFAULT';
IF:                                  'IF';
EXISTS:                              'EXISTS';
IS:                                  'IS';
BETWEEN:                             'BETWEEN';
AND:                                 'AND';
OR:                                  'OR';


// DATA TYPE Keywords

TINYINT:                             'TINYINT';
SMALLINT:                            'SMALLINT';
MEDIUMINT:                           'MEDIUMINT';
INT:                                 'INT';
INTEGER:                             'INTEGER';
BIGINT:                              'BIGINT';
REAL:                                'REAL';
DOUBLE:                              'DOUBLE';
PRECISION:                           'PRECISION';
FLOAT:                               'FLOAT';
DECIMAL:                             'DECIMAL';
DEC:                                 'DEC';
NUMERIC:                             'NUMERIC';
CHAR:                                'CHAR';
VARCHAR:                             'VARCHAR';
BINARY:                              'BINARY';
VARBINARY:                           'VARBINARY';
TINYBLOB:                            'TINYBLOB';
BLOB:                                'BLOB';
MEDIUMBLOB:                          'MEDIUMBLOB';
LONGBLOB:                            'LONGBLOB';
TINYTEXT:                            'TINYTEXT';
TEXT:                                'TEXT';
MEDIUMTEXT:                          'MEDIUMTEXT';
LONGTEXT:                            'LONGTEXT';
VARYING:                             'VARYING';
SERIAL:                              'SERIAL';
STRING:                              'STRING';
BOOL:                                'BOOL';
BOOLEAN:                             'BOOLEAN';
LIST:                                'LIST';
STRING_LIST:                         'STRING_LIST';
NUMBER_LIST:                         'NUMBER_LIST';

// Keywords, but can be ID
// Common Keywords, but can be ID

COLUMNS:                             'COLUMNS';
FIELDS:                              'FIELDS';
INDEXES:                             'INDEXES';
VALUE:                               'VALUE';
TRUNCATE:                            'TRUNCATE';
HASH:                                'HASH';
THROUGHPUT:                          'THROUGHPUT';
BILLINGMODE:                         'BILLINGMODE';
PROVISIONED:                         'PROVISIONED';
PAY_PER_REQUEST:                     'PAY_PER_REQUEST';

// PRIVILEGES

TABLES:                              'TABLES';

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
NULL_SPEC_LITERAL:                   '\\' 'N';
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
