parser grammar PrimarSqlParser;

options {
    tokenVocab=PrimarSqlLexer;
}

root
    : sqlStatements? MINUSMINUS? EOF
    ;

sqlStatements
    : (sqlStatement MINUSMINUS? SEMI? | emptyStatement)*
    (sqlStatement (MINUSMINUS? SEMI)? | emptyStatement)
    ;

sqlStatement
    : ddlStatement | dmlStatement
    | describeStatement | showStatement
    ;

emptyStatement
    : SEMI
    ;

ddlStatement
    : createIndex | createTable | alterTable
    | dropIndex | dropTable
    ;

dmlStatement
    : selectStatement | insertStatement 
    | updateStatement | deleteStatement
    ;

// ddl statement

createIndex
    : CREATE indexSpec? INDEX uid
    ON tableName primaryKeyColumns
    indexOption?
    ;

indexSpec
    : LOCAL | GLOBAL
    ;

indexOption
    : ALL
    | KEYS ONLY
    | INCLUDE '(' uid (',' uid)* ')'
    ;

createTable
    : CREATE TABLE ifNotExists?
       tableName createDefinitions
       ( tableOption (','? tableOption)* )?
    ;

createDefinitions
    : '(' createDefinition (',' createDefinition)* ')'
    ;

createDefinition
    : uid columnDefinition                                          #columnDeclaration
    | tableConstraint                                               #constraintDeclaration
    | indexColumnDefinition                                         #indexDeclaration
    ;

columnDefinition
    : dataType columnConstraint*
    ;

columnConstraint
    : HASH KEY                                                      #hashKeyColumnConstraint
    | RANGE KEY                                                     #rangeKeyColumnConstraint
    ;

tableConstraint
    : HASH KEY index=uid?                                           #hashKeyTableConstraint
    | RANGE KEY index=uid?                                          #rangeKeyTableConstraint
    ;

indexColumnDefinition
    : indexType=(LOCAL | GLOBAL)? INDEX 
        uid primaryKeyColumns indexOption?
    ;

tableOption
    : THROUGHPUT '='? '(' 
        readCapacity=decimalLiteral ',' 
        writeCapacity=decimalLiteral ')'                                      #tableOptionThroughput
    | BILLINGMODE '='? billingMode=(PROVISIONED | PAY_PER_REQUEST| ON_DEMAND) #tableBillingMode
    ;

alterTable
    : ALTER TABLE tableName 
       (alterSpecification (',' alterSpecification)*)?
    ;

alterSpecification
    : tableOption                                                   #alterTableOption
    | ADD indexColumnDefinition                                     #alterAddIndex
    | ALTER INDEX indexName=uid THROUGHPUT '='? '(' 
        readCapacity=decimalLiteral ',' 
        writeCapacity=decimalLiteral ')'                            #alterIndexThroughput
    | DROP INDEX indexName=uid                                      #alterDropIndex
    ;

dropIndex
    : DROP INDEX indexName=uid ON tableName
    ;
    
dropTable
    : DROP TABLE ifExists? tableName (',' tableName)* 
    ;

// dml statement

insertStatement
    : INSERT IGNORE? INTO? tableName
        ('(' columns=uidList ')')? insertStatementValue
    ;

selectStatement
    : querySpecification                                    #simpleSelect
    | queryExpression                                       #parenthesisSelect
    ;

insertStatementValue
    : selectStatement
    | insertFormat=(VALUES | VALUE)
      '(' expressionsWithDefaults? ')'
        (',' '(' expressionsWithDefaults? ')')*
    ;

orderClause
    : ORDER order=(ASC | DESC)
    ;

tableSource
    : tableSourceItem                                       #tableSourceBase
    | '(' tableSourceItem ')'                               #tableSourceNested
    ;

tableSourceItem
    : tableName (AS? alias=uid)?                                    #atomTableItem
    | '(' parenthesisSubquery=selectStatement ')' (AS? alias=uid)?  #subqueryTableItem
    ;

// Select Statement

queryExpression
    : '(' querySpecification ')'
    | '(' queryExpression ')'
    ;

querySpecification
    : SELECT selectSpec? selectElements
      fromClause? orderClause? limitClause? offsetClause? startKeyClause?
    ;

selectSpec
    : STRONGLY
    | EVENTUALLY
    ;
    
selectElements
    : (star='*' | selectElement)* (',' selectElement)*
    ;
    
selectElement
    : fullColumnName (AS? alias=uid)?                       #selectColumnElement
    | functionCall (AS? alias=uid)?                         #selectFunctionElement
    ;
    
fromClause
    : FROM tableSource
      (WHERE whereExpr=expression)?
//      (
//        GROUP BY
//        groupByItem (',' groupByItem)*
//        (WITH ROLLUP)?
//      )?
//      (HAVING havingExpr=expression)?
    ;

limitClause
    : LIMIT limit=decimalLiteral
    ;

offsetClause
    : OFFSET offset=decimalLiteral
    ;

startKeyClause
    : START KEY '(' hashKey=constant (',' sortKey=constant)? ')'
    ;

// update statements

updatedElement
    : fullColumnName '=' (expression | DEFAULT)
    ;

updateStatement
    : UPDATE tableName
        SET updatedElement (',' updatedElement)*
        (WHERE expression)? limitClause?
    ;

// delete statements

deleteStatement
    : DELETE FROM tableName
        (WHERE expression)? limitClause?
    ;

// describe statements

describeStatement
    : (DESCRIBE | DESC) describeSpecification
    ;

describeSpecification
    : TABLE tableName                                      #describeTable
    | LIMITS                                               #describeLimits
    | ENDPOINTS                                            #describeEndPoints
    ;

// show statements

showStatement
    : SHOW showSpecification
    ;

showSpecification
    : TABLES                                               #showTables
    | (INDEX | INDEXES) (FROM | IN) tableName              #showIndexes
    ;    

fullId
    : uid dottedId?
    ;

tableName
    : fullId
    ;

fullColumnName
    : uid (dottedId dottedId? )?
    ;

// DB Objects

columnName
    : (uid | STRING_LITERAL)
    ;

uid
    : simpleId
    | DOUBLE_QUOTE_ID
    | REVERSE_QUOTE_ID
    ;

simpleId
    : ID
    | STRING_LITERAL
    | keywordsCanBeId
    ;

dottedId
    : '.' uid
    ;

//    Literals

decimalLiteral
    : DECIMAL_LITERAL | ZERO_DECIMAL | ONE_DECIMAL | TWO_DECIMAL
    ;

stringLiteral
    : (
        STRING_LITERAL
      ) STRING_LITERAL*
    ;

booleanLiteral
    : TRUE | FALSE;

hexadecimalLiteral
    : HEXADECIMAL_LITERAL;

nullLiteral
    : NULL_LITERAL | NULL_SPEC_LITERAL;

nullNotnull
    : NOT? nullLiteral
    ;

constant
    : stringLiteral                                                 #stringLiteralConstant
    | decimalLiteral                                                #positiveDecimalLiteralConstant
    | '-' decimalLiteral                                            #negativeDecimalLiteralConstant
    | hexadecimalLiteral                                            #hexadecimalLiteralConstant
    | booleanLiteral                                                #booleanLiteralConstant
    | REAL_LITERAL                                                  #realLiteralConstant
    | BIT_STRING                                                    #bitStringConstant
    | nullNotnull                                                   #nullLiteralConstant
    ;

//    Common Lists

uidList
    : uid (',' uid)*
    ;

primaryKeyColumns
    : '(' hashKey=columnName (',' sortKey=columnName)? ')'
    ;

//    Data Types

dataType
    : typeName=(
      VARCHAR | TEXT | MEDIUMTEXT | LONGTEXT | STRING
      | INT | INTEGER | BIGINT | BOOL | BOOLEAN | LIST
      | BINARY | NUMBER_LIST | STRING_LIST | BINARY_LIST
      )
    ;
    

//    Functions

functionCall
    : updateItemFunction                                            #updateItemFunctionCall
    | conditionExpressionFunction                                   #conditionExpressionFunctionCall
    ;

updateItemFunction
    : IF_NOT_EXISTS '(' dottedId separator=',' constant ')'         #ifNotExistsFunctionCall
    ;

conditionExpressionFunction
    : ATTRIBUTE_EXISTS '(' dottedId ')'                             #attributeExistsFunctionCall
    | ATTRIBUTE_NOT_EXISTS '(' dottedId ')'                         #attributeNotExistsFunctionCall
    | ATTRIBUTE_TYPE '(' dottedId separator=',' dataType ')'        #attributeTypeFunctionCall
    | BEGINS_WITH '(' dottedId separator=',' stringLiteral ')'      #beginsWithFunctionCall
    | CONTAINS '(' dottedId separator=',' stringLiteral ')'         #beginsWithFunctionCall
    | SIZE '(' dottedId ')'                                         #beginsWithFunctionCall
    ;
    
ifExists
    : IF EXISTS;
    
ifNotExists
    : IF NOT EXISTS;

expressionsWithDefaults
    : expressionOrDefault (',' expressionOrDefault)*
    ;

expressions
    : expression (',' expression)*
    ;

expressionOrDefault
    : expression | DEFAULT
    ;

expression
    : notOperator=(NOT | '!') expression                            #notExpression
    | left=expression logicalOperator right=expression              #logicalExpression
    | predicate                                                     #predicateExpression
    ;

predicate
    : predicate NOT? IN '(' (selectStatement | expressions) ')'     #inPredicate
    | predicate IS nullNotnull                                      #isNullPredicate
    | left=predicate comparisonOperator right=predicate             #binaryComparasionPredicate
    | predicate comparisonOperator
      quantifier=(ALL | ANY | SOME) '(' selectStatement ')'         #subqueryComparasionPredicate
    | predicate NOT? BETWEEN predicate AND predicate                #betweenPredicate
    | left=predicate SOUNDS LIKE right=predicate                    #soundsLikePredicate
    | left=predicate NOT? LIKE right=predicate
      (ESCAPE STRING_LITERAL)?                                      #likePredicate
    | left=predicate NOT? regex=(REGEXP | RLIKE) right=predicate    #regexpPredicate
    | expressionAtom                                                #expressionAtomPredicate
    ;

expressionAtom
    : constant                                                      #constantExpressionAtom
    | fullColumnName                                                #fullColumnNameExpressionAtom
    | functionCall                                                  #functionCallExpressionAtom
    | '(' expression (',' expression)* ')'                          #nestedExpressionAtom
    | ROW '(' expression (',' expression)+ ')'                      #nestedRowExpressionAtom
    | EXISTS '(' selectStatement ')'                                #existsExpessionAtom
    | '(' selectStatement ')'                                       #subqueryExpessionAtom
    | left=expressionAtom bitOperator right=expressionAtom          #bitExpressionAtom
    | left=expressionAtom mathOperator right=expressionAtom         #mathExpressionAtom
    ;

comparisonOperator
    : '=' | '>' | '<' | '<' '=' | '>' '='
    | '<' '>' | '!' '=' | '<' '=' '>'
    ;

logicalOperator
    : AND | '&' '&' | XOR | OR | '|' '|'
    ;

bitOperator
    : '<' '<' | '>' '>' | '&' | '^' | '|'
    ;

mathOperator
    : '*' | '/' | '%' | DIV | MOD | '+' | '-' | '--'
    ;


keywordsCanBeId
    : SELECT
    | STRONGLY
    | EVENTUALLY
    | AS
    | FROM
    | WHERE
    | GROUP
    | BY
    | WITH
    | ROLLUP
    | HAVING
    | LIMIT
    | LIMITS
    | OFFSET
    | TRUE
    | FALSE
    | VARCHAR
    | TEXT
    | MEDIUMTEXT
    | LONGTEXT
    | STRING
    | INT
    | INTEGER
    | BIGINT
    | BOOL
    | BOOLEAN
    | LIST
    | BINARY
    | NUMBER_LIST
    | STRING_LIST
    | BINARY_LIST
    | ORDER
    | CREATE
    | INDEX
    | INDEXES
    | ON
    | LOCAL
    | GLOBAL
    | ALL
    | KEYS
    | ONLY
    | INCLUDE
    | TABLE
    | TABLES
    | HASH
    | KEY
    | RANGE
    | THROUGHPUT
    | BILLINGMODE
    | PROVISIONED
    | PAY_PER_REQUEST
    | ON_DEMAND
    | ALTER
    | ADD
    | DROP
    | INSERT
    | IGNORE
    | INTO
    | VALUES
    | VALUE
    | ASC
    | DESC
    | DESCRIBE
    | NULL_LITERAL
    | NOT
    | IF_NOT_EXISTS
    | ATTRIBUTE_EXISTS
    | ATTRIBUTE_NOT_EXISTS
    | ATTRIBUTE_TYPE
    | BEGINS_WITH
    | CONTAINS
    | SIZE
    | IF
    | EXISTS
    | DEFAULT
    | BETWEEN
    | AND
    | SOUNDS
    | LIKE
    | REGEXP
    | RLIKE
    | IN
    | IS
    | ANY
    | SOME
    | ESCAPE
    | ROW
    | XOR
    | OR
    | START
    | ENDPOINTS
    | SHOW
    | DELETE
    | UPDATE
    | SET
    ;
