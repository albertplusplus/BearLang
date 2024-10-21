namespace BearLang;

abstract class Expr
{

    public abstract R accept<R>(Visitor<R> visitor);

    public interface Visitor<R>
    {
        R visitBinaryExpr(Binary expr);
        R visitGroupingExpr(Grouping expr);
        R visitLiteralExpr(Literal expr);
        R visitUnaryExpr(Unary expr);
    }


    public class Binary : Expr
    {
        public readonly Expr left;
        public readonly Token op;
        public readonly Expr right;

        public Binary(Expr left, Token op, Expr right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override R accept<R>(Visitor<R> visitor)
        {
            return visitor.visitBinaryExpr(this);
        }
    }

    public class Grouping : Expr
    {
        public readonly Expr expr;
        public Grouping(Expr expr)
        {
            this.expr = expr;
        }
        public override R accept<R>(Visitor<R> visitor)
        {
            return visitor.visitGroupingExpr(this);
        }
    }

    public class Literal : Expr
    {
        public readonly Object val;
        public Literal(Object val)
        {
            this.val = val;
        }
        public override R accept<R>(Visitor<R> visitor)
        {
            return visitor.visitLiteralExpr(this);
        }
    }

    public class Unary : Expr
    {
        public readonly Token op;
        public readonly Expr right;
        public Unary(Token op, Expr right)
        {
            this.op = op;
            this.right = right;
        }
        public override R accept<R>(Visitor<R> visitor)
        {
            return visitor.visitUnaryExpr(this);
        }
    }

}
