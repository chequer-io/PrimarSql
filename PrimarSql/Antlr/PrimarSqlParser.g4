parser grammar PrimarSqlParser;

options {
    tokenVocab=PrimarSqlLexer;
}

root
    : sqlStatement SEMI? EOF
    ;

sqlStatement
    : ddlStatement | dmlStatement
    | describeStatement | showStatement
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
    : (HASH | PARTITION) KEY                                        #hashKeyColumnConstraint
    | (RANGE | SORT) KEY                                            #rangeKeyColumnConstraint
    ;

tableConstraint
    : (HASH | PARTITION) KEY index=uid?                             #hashKeyTableConstraint
    | (RANGE | SORT) KEY index=uid?                                 #rangeKeyTableConstraint
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
    : (star='*' | selectElement (',' selectElement)*)
    ;
    
selectElement
    : fullColumnName (AS? alias=uid)?                       #selectColumnElement
    | builtInFunctionCall (AS? alias=uid)?                  #selectFunctionElement
    | expression (AS? alias=uid)?                           #selectExpressionElement
    ;

fromClause
    : FROM tableSource
      (WHERE whereExpr=expression)?
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
    : NULL_LITERAL;

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
    | predicate NOT? BETWEEN predicate AND predicate                #betweenPredicate
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
    | EXISTS '(' selectStatement ')'                                #existsExpressionAtom
    | '(' selectStatement ')'                                       #subqueryExpressionAtom
    | left=expressionAtom bitOperator right=expressionAtom          #bitExpressionAtom
    | left=expressionAtom mathOperator right=expressionAtom         #mathExpressionAtom
    ;

//    Functions

functionCall
    : builtInFunctionCall
    | nativeFunctionCall
    ; 

builtInFunctionCall
    : ( CURRENT_DATE | CURRENT_TIME | CURRENT_TIMESTAMP )           #timeFunctionCall
    | CAST '(' expression AS dataType ')'                           #castFunctionCall
    | (SUBSTR | SUBSTRING)
        '('
            (
                sourceString=stringLiteral
                | sourceExpression=expression
            ) FROM
            (
                fromDecimal=decimalLiteral
                | fromExpression=expression
            )
            (
                FOR
                (
                    forDecimal=decimalLiteral
                    | forExpression=expression
                )
            )?
        ')'                                                          #substrFunctionCall
    | TRIM
        '('
            positioinForm=(BOTH | LEADING | TRAILING)
            (
                sourceString=stringLiteral
                | sourceExpression=expression
            )?
            FROM
            (
                fromString=stringLiteral
                | fromExpression=expression
            )
        ')'                                                           #trimFunctionCall
    | TRIM
        '('
            (
                sourceString=stringLiteral
                | sourceExpression=expression
            )
            FROM
            (
                fromString=stringLiteral
                | fromExpression=expression
            )
        ')'                                                           #trimFunctionCall
    ;

nativeFunctionCall
    : updateItemFunction                                            #updateItemFunctionCall
    | conditionExpressionFunction                                   #conditionExpressionFunctionCall
    ;

updateItemFunction
    : IF_NOT_EXISTS '(' fullId separator=',' constant ')'         #ifNotExistsFunctionCall
    ;

conditionExpressionFunction
    : ATTRIBUTE_EXISTS '(' fullId ')'                             #attributeExistsFunctionCall
    | ATTRIBUTE_NOT_EXISTS '(' fullId ')'                         #attributeNotExistsFunctionCall
    | ATTRIBUTE_TYPE '(' fullId separator=',' dataType ')'        #attributeTypeFunctionCall
    | BEGINS_WITH '(' fullId separator=',' stringLiteral ')'      #beginsWithFunctionCall
    | CONTAINS '(' fullId separator=',' stringLiteral ')'         #beginsWithFunctionCall
    | SIZE '(' fullId ')'                                         #beginsWithFunctionCall
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
