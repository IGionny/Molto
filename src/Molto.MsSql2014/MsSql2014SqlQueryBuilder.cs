using System;
using System.Linq;
using Molto.Abstractions;

namespace Molto.MsSql2014
{
    public class MsSql2014SqlQueryBuilder : SqlQueryBuilder
    {
        public MsSql2014SqlQueryBuilder(IEntityDatabaseMapProvider entityDatabaseMapProvider,
            ISqlQueryCutter sqlQueryCutter) : base(entityDatabaseMapProvider, sqlQueryCutter)
        {
        }


        /// <summary>
        /// From: https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/ms189822(v=sql.105)
        /// </summary>
        /// <jsScript>
        /// var output = [];
        /// $(".table-scroll-wrapper tbody td").each(function (i, item){
        /// output.push("\"" + $(item).text().toLowerCase() + "\"");
        /// });
        /// console.log(output.join(","));
        /// </jsScript>
        public override string[] ReservedWords => new[]
        {
            "add", "exists", "precision", "all", "exit", "primary", "alter", "external", "print", "and", "fetch",
            "proc", "any", "file", "procedure", "as", "fillfactor", "public", "asc", "for", "raiserror",
            "authorization", "foreign", "read", "backup", "freetext", "readtext", "begin", "freetexttable",
            "reconfigure", "between", "from", "references", "break", "full", "replication", "browse", "function",
            "restore", "bulk", "goto", "restrict", "by", "grant", "return", "cascade", "group", "revert", "case",
            "having", "revoke", "check", "holdlock", "right", "checkpoint", "identity", "rollback", "close",
            "identity_insert", "rowcount", "clustered", "identitycol", "rowguidcol", "coalesce", "if", "rule",
            "collate", "in", "save", "column", "index", "schema", "commit", "inner", "securityaudit", "compute",
            "insert", "select", "constraint", "intersect", "session_user", "contains", "into", "set", "containstable",
            "is", "setuser", "continue", "join", "shutdown", "convert", "key", "some", "create", "kill", "statistics",
            "cross", "left", "system_user", "current", "like", "table", "current_date", "lineno", "tablesample",
            "current_time", "load", "textsize", "current_timestamp", "merge", "then", "current_user", "national", "to",
            "cursor", "nocheck", "top", "database", "nonclustered", "tran", "dbcc", "not", "transaction", "deallocate",
            "null", "trigger", "declare", "nullif", "truncate", "default", "of", "tsequal", "delete", "off", "union",
            "deny", "offsets", "unique", "desc", "on", "unpivot", "disk", "open", "update", "distinct",
            "opendatasource", "updatetext", "distributed", "openquery", "use", "double", "openrowset", "user", "drop",
            "openxml", "values", "dump", "option", "varying", "else", "or", "view", "end", "order", "waitfor", "errlvl",
            "outer", "when", "escape", "over", "where", "except", "percent", "while", "exec", "pivot", "with",
            "execute", "plan", "writetext", "absolute", "exec", "overlaps", "action", "execute", "pad", "ada", "exists",
            "partial", "add", "external", "pascal", "all", "extract", "position", "allocate", "false", "precision",
            "alter", "fetch", "prepare", "and", "first", "preserve", "any", "float", "primary", "are", "for", "prior",
            "as", "foreign", "privileges", "asc", "fortran", "procedure", "assertion", "found", "public", "at", "from",
            "read", "authorization", "full", "real", "avg", "get", "references", "begin", "global", "relative",
            "between", "go", "restrict", "bit", "goto", "revoke", "bit_length", "grant", "right", "both", "group",
            "rollback", "by", "having", "rows", "cascade", "hour", "schema", "cascaded", "identity", "scroll", "case",
            "immediate", "second", "cast", "in", "section", "catalog", "include", "select", "char", "index", "session",
            "char_length", "indicator", "session_user", "character", "initially", "set", "character_length", "inner",
            "size", "check", "input", "smallint", "close", "insensitive", "some", "coalesce", "insert", "space",
            "collate", "int", "sql", "collation", "integer", "sqlca", "column", "intersect", "sqlcode", "commit",
            "interval", "sqlerror", "connect", "into", "sqlstate", "connection", "is", "sqlwarning", "constraint",
            "isolation", "substring", "constraints", "join", "sum", "continue", "key", "system_user", "convert",
            "language", "table", "corresponding", "last", "temporary", "count", "leading", "then", "create", "left",
            "time", "cross", "level", "timestamp", "current", "like", "timezone_hour", "current_date", "local",
            "timezone_minute", "current_time", "lower", "to", "current_timestamp", "match", "trailing", "current_user",
            "max", "transaction", "cursor", "min", "translate", "date", "minute", "translation", "day", "module",
            "trim", "deallocate", "month", "true", "dec", "names", "union", "decimal", "national", "unique", "declare",
            "natural", "unknown", "default", "nchar", "update", "deferrable", "next", "upper", "deferred", "no",
            "usage", "delete", "none", "user", "desc", "not", "using", "describe", "null", "value", "descriptor",
            "nullif", "values", "diagnostics", "numeric", "varchar", "disconnect", "octet_length", "varying",
            "distinct", "of", "view", "domain", "on", "when", "double", "only", "whenever", "drop", "open", "where",
            "else", "option", "with", "end", "or", "work", "end-exec", "order", "write", "escape", "outer", "year",
            "except", "output", "zone", "exception", " ", " ", "absolute", "host", "relative", "action", "hour",
            "release", "admin", "ignore", "result", "after", "immediate", "returns", "aggregate", "indicator", "role",
            "alias", "initialize", "rollup", "allocate", "initially", "routine", "are", "inout", "row", "array",
            "input", "rows", "asensitive", "int", "savepoint", "assertion", "integer", "scroll", "asymmetric",
            "intersection", "scope", "at", "interval", "search", "atomic", "isolation", "second", "before", "iterate",
            "section", "binary", "language", "sensitive", "bit", "large", "sequence", "blob", "last", "session",
            "boolean", "lateral", "sets", "both", "leading", "similar", "breadth", "less", "size", "call", "level",
            "smallint", "called", "like_regex", "space", "cardinality", "limit", "specific", "cascaded", "ln",
            "specifictype", "cast", "local", "sql", "catalog", "localtime", "sqlexception", "char", "localtimestamp",
            "sqlstate", "character", "locator", "sqlwarning", "class", "map", "start", "clob", "match", "state",
            "collation", "member", "statement", "collect", "method", "static", "completion", "minute", "stddev_pop",
            "condition", "mod", "stddev_samp", "connect", "modifies", "structure", "connection", "modify",
            "submultiset", "constraints", "module", "substring_regex", "constructor", "month", "symmetric", "corr",
            "multiset", "system", "corresponding", "names", "temporary", "covar_pop", "natural", "terminate",
            "covar_samp", "nchar", "than", "cube", "nclob", "time", "cume_dist", "new", "timestamp", "current_catalog",
            "next", "timezone_hour", "current_default_transform_group", "no", "timezone_minute", "current_path", "none",
            "trailing", "current_role", "normalize", "translate_regex", "current_schema", "numeric", "translation",
            "current_transform_group_for_type", "object", "treat", "cycle", "occurrences_regex", "true", "data", "old",
            "uescape", "date", "only", "under", "day", "operation", "unknown", "dec", "ordinality", "unnest", "decimal",
            "out", "usage", "deferrable", "overlay", "using", "deferred", "output", "value", "depth", "pad", "var_pop",
            "deref", "parameter", "var_samp", "describe", "parameters", "varchar", "descriptor", "partial", "variable",
            "destroy", "partition", "whenever", "destructor", "path", "width_bucket", "deterministic", "postfix",
            "without", "dictionary", "prefix", "window", "diagnostics", "preorder", "within", "disconnect", "prepare",
            "work", "domain", "percent_rank", "write", "dynamic", "percentile_cont", "xmlagg", "each",
            "percentile_disc", "xmlattributes", "element", "position_regex", "xmlbinary", "end-exec", "preserve",
            "xmlcast", "equals", "prior", "xmlcomment", "every", "privileges", "xmlconcat", "exception", "range",
            "xmldocument", "false", "reads", "xmlelement", "filter", "real", "xmlexists", "first", "recursive",
            "xmlforest", "float", "ref", "xmliterate", "found", "referencing", "xmlnamespaces", "free", "regr_avgx",
            "xmlparse", "fulltexttable", "regr_avgy", "xmlpi", "fusion", "regr_count", "xmlquery", "general",
            "regr_intercept", "xmlserialize", "get", "regr_r2", "xmltable", "global", "regr_slope", "xmltext", "go",
            "regr_sxx", "xmlvalidate", "grouping", "regr_sxy", "year", "hold", "regr_syy", "zone"
        };

        public override string PageSql<T>(string sql, long page, long itemsPerPage, long resultTotalItems)
        {
            if (!sql.ToUpper().Contains("ORDER BY"))
                throw new Exception("SQL Server 2012 Paging via OFFSET requires an ORDER BY statement.");

            var skip = (page - 1) * itemsPerPage;
            sql = sql + $" OFFSET {skip}  ROWS FETCH NEXT {itemsPerPage}  ROWS ONLY";
            return sql;
        }

        public override string SingleSql(string sql)
        {
            sql = sql.Trim().Substring(Sql.Select.Length);
            sql = "SELECT TOP 1 " + sql;
            return sql;
        }
    }
}