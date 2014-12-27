using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core.Generation
{
    /// <summary>
    /// This class provides code generation capabilities
    /// </summary>
    public static partial class Generator
    {
        public enum TypeAccessModifier
        {
            Unspecified,
            Internal,
            Public
        }

        public enum MemberAccessModifier
        {
            Unspecified,
            Private,
            Protected,
            Protected_Internal,
            Internal,
            Public
        }

        public static SyntaxNode Namespace(string name, IEnumerable<TypeDeclarationSyntax> types = null)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(name));
        }


        public static InterfaceDeclarationSyntax Interface(string name, TypeAccessModifier access = TypeAccessModifier.Unspecified)
        {
            return SyntaxFactory.InterfaceDeclaration(name)
                .AddModifiers(SyntaxKind.PublicKeyword.ToToken());
        }

        public static NameSyntax Name(string name)
        {
            return SyntaxFactory.ParseName(name);
        }

        public static TypeSyntax TypeName(string type)
        {
            return SyntaxFactory.ParseTypeName(type);
        }



        /// <summary>
        /// Construct a statement block with given statements
        /// </summary>
        public static BlockSyntax Block(SyntaxList<StatementSyntax> statements = default(SyntaxList<StatementSyntax>))
        {
            return SyntaxFactory.Block(
                SyntaxKind.OpenBraceToken.ToToken(),
                statements,
                SyntaxKind.CloseBraceToken.ToToken()
            );
        }

        #region Parameters

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="type">The type of the parameter</param>
        /// <param name="attributeLists">Custom attributes?</param>
        /// <param name="modifiers">ref/out modifiers?</param>
        public static ParameterSyntax Parameter(string name, string type, SyntaxList<AttributeListSyntax> attributeLists = default(SyntaxList<AttributeListSyntax>), SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return Parameter(name, SyntaxFactory.ParseTypeName(type), attributeLists, modifiers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="attributeLists"></param>
        /// <param name="modifiers"></param>
        public static ParameterSyntax Parameter(string name, TypeSyntax type, SyntaxList<AttributeListSyntax> attributeLists = default(SyntaxList<AttributeListSyntax>), SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return SyntaxFactory.Parameter(
                attributeLists,
                modifiers,
                type,
                SyntaxFactory.Identifier(name),
                null
            );
        }

        public static ParameterListSyntax Parameters()
        {
            return Parameters(new ParameterSyntax[] { });
        }

        public static ParameterListSyntax Parameters(ParameterSyntax p1)
        {
            return Parameters(new[] { p1 });
        }

        public static ParameterListSyntax Parameters(ParameterSyntax p1, ParameterSyntax p2)
        {
            return Parameters(new[] { p1, p2 });
        }

        public static ParameterListSyntax Parameters(params ParameterSyntax[] parameters)
        {
            return SyntaxFactory.ParameterList().AddParameters(parameters);
        }

        #endregion

#region Arrays

        /// <summary>
        /// Creates an array of specified type
        /// </summary>
        /// <param name="type">The type of the array</param>
        /// <returns></returns>
        public static ArrayTypeSyntax Array(string type)
        {
            return Array(SyntaxFactory.ParseTypeName(type))
                .WithRankSpecifiers(
                SyntaxFactory.List<ArrayRankSpecifierSyntax>().Add(
                    SyntaxFactory.ArrayRankSpecifier()
                )
            );
        }

        public static ArrayTypeSyntax Array(TypeSyntax type)
        {
            return SyntaxFactory.ArrayType(type);
        }

        public static ArrayCreationExpressionSyntax Array(string type, params ArrayRankSpecifierSyntax[] ranks)
        {
            return Array(SyntaxFactory.ParseTypeName(type), ranks);
        }

        public static ArrayCreationExpressionSyntax Array(TypeSyntax type, params ArrayRankSpecifierSyntax[] ranks)
        {
            return SyntaxFactory.ArrayCreationExpression(Array("string"));
        }

#endregion

        public static IfStatementSyntax If()
        {
            return SyntaxFactory.IfStatement(
                SyntaxFactory.ParseExpression("true"),
                SyntaxFactory.EmptyStatement());
        }

        public static ReturnStatementSyntax Return(ExpressionSyntax expression = null)
        {
            return SyntaxFactory.ReturnStatement()
                .WithExpression(expression);


            //.with (SyntaxFactory.Token(SyntaxKind.ReturnKeyword),expression:,;
        }

        public static WhileStatementSyntax While(ExpressionSyntax condition, StatementSyntax statement)
        {
            return SyntaxFactory.WhileStatement(condition, statement);
        }

        public static ForStatementSyntax For(StatementSyntax statement)
        {
            return SyntaxFactory.ForStatement(statement);
        }

        public static AssignmentExpressionSyntax Equals()
        {
            throw new NotImplementedException();
            //return SyntaxFactory.AssignmentExpression(SyntaxKind left, ExpressionSyntax left right);
        }

        /*public static ForEachStatementSyntax ForEach() {
			return SyntaxFactory.ForEachStatement(Type,identifier:,ExpressionSyntax:,StatementSyntax);
		}*/

        public static EmptyStatementSyntax Statement()
        {
            return SyntaxFactory.EmptyStatement();
        }

        public static UsingStatementSyntax Using(VariableDeclarationSyntax declaration = default(VariableDeclarationSyntax), ExpressionSyntax expression = null, StatementSyntax statement = default(StatementSyntax))
        {
            return SyntaxFactory.UsingStatement(
                SyntaxKind.UsingKeyword.ToToken(),
                SyntaxKind.OpenParenToken.ToToken(),
                declaration,
                expression,
                SyntaxKind.CloseParenToken.ToToken(),
                statement);
        }

        public static VariableDeclaratorSyntax Variable(string name)
        {
            return SyntaxFactory.VariableDeclarator(name);
        }

        public static VariableDeclarationSyntax Variable(string type, string name)
        {
            return Variable(SyntaxFactory.ParseTypeName(type), name);
        }

        public static VariableDeclarationSyntax Variable(TypeSyntax type, string value)
        {
            return SyntaxFactory.VariableDeclaration(type, SyntaxFactory.SeparatedList<VariableDeclaratorSyntax>()
                .Add(Variable(value)));
        }

        /*public static VariableDeclarationSyntax Variable(KeyValuePair<string,TypeSyntax> variables)
        {
            return SyntaxFactory.VariableDeclaration(t)
        }*/

        public static ObjectCreationExpressionSyntax ObjectCreation(TypeSyntax type)
        {
            return SyntaxFactory.ObjectCreationExpression(type);
        }

        public static InitializerExpressionSyntax Initializer(SeparatedSyntaxList<ExpressionSyntax> expressions = default(SeparatedSyntaxList<ExpressionSyntax>))
        {
            return SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, expressions);
        }

        public static InvocationExpressionSyntax Invoke(ExpressionSyntax expression = default(ExpressionSyntax), ArgumentListSyntax arguments = default(ArgumentListSyntax))
        {
            return SyntaxFactory.InvocationExpression(expression, SyntaxFactory.ArgumentList()
                .AddArguments(new ArgumentSyntax[] { SyntaxFactory.Argument(Generator.Literal("Hello")) }));
        }

        /*public static RefValueExpressionSyntax ObjectRef(){
			return SyntaxFactory.RefValueExpression (SyntaxFactory.ParseExpression ("Console"), TypeName ("W"));
		}*/

        public static ExpressionSyntax ParseExpression(string expression)
        {
            return SyntaxFactory.ParseExpression(expression);
        }

        public static MemberAccessExpressionSyntax[] Members(params MemberAccessExpressionSyntax[] members)
        {
            return members;
        }

        public static MemberAccessExpressionSyntax Member(string name, ExpressionSyntax expression, SyntaxKind kind = SyntaxKind.SimpleMemberAccessExpression)
        {
            return SyntaxFactory.MemberAccessExpression(
                kind,
                expression,
                SyntaxFactory.IdentifierName(name));
        }

        /*public static MemberBindingExpressionSyntax Member(string name){
			return SyntaxFactory.MemberBindingExpression (SyntaxFactory.IdentifierName (name));
		}*/

        public static NameOfExpressionSyntax Name()
        {
            return SyntaxFactory.NameOfExpression("WriteLine", SyntaxFactory.ParseExpression("\"test\""));
        }

        public static TypeOfExpressionSyntax TypeOf(TypeSyntax type, SyntaxToken keyword = default(SyntaxToken))
        {
            return SyntaxFactory.TypeOfExpression(
                keyword,
                SyntaxKind.OpenParenToken.ToToken(),
                type,
                SyntaxKind.CloseBraceToken.ToToken()
            );
        }
    }
}
