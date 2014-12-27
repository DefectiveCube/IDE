using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core.Generation
{
    public static partial class Generator
    {
        public static ClassDeclarationSyntax Class(
    string name,
    TypeAccessModifier access = TypeAccessModifier.Unspecified,
    string baseClass = "System.Object",
    TypeSyntax parent = null,
    bool isAbstract = false,
    bool isSealed = false,
    bool isStatic = false,
    IEnumerable<MemberDeclarationSyntax> members = null,
    IEnumerable<string> interfaces = null,
    IEnumerable<string> attributes = null)
        {
            var clsDec = SyntaxFactory.ClassDeclaration(name)
                .WithOpenBraceToken(SyntaxKind.OpenBraceToken)
                .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(members))
                .WithCloseBraceToken(SyntaxKind.CloseBraceToken);

            // Determine the access modifier
            switch (access)
            {
                case TypeAccessModifier.Public:
                    clsDec = clsDec.WithKeyword(SyntaxKind.PublicKeyword);
                    break;
                case TypeAccessModifier.Internal:
                    clsDec = clsDec.WithKeyword(SyntaxKind.InternalKeyword);
                    break;
                default:
                    // Not specified
                    break;
            }

            // Set modifier keywords
            clsDec = isAbstract ? clsDec.WithKeyword(SyntaxKind.AbstractKeyword) : clsDec;
            clsDec = isSealed ? clsDec.WithKeyword(SyntaxKind.SealedKeyword) : clsDec;
            clsDec = isStatic ? clsDec.WithKeyword(SyntaxKind.StaticKeyword) : clsDec;

            var baseList = SyntaxFactory.BaseList();

            // Add base type
            if (parent != null)
            {
                baseList = baseList.AddTypes(new[] { SyntaxFactory.SimpleBaseType(parent) });
            }

            // Implemented Interfaces
            if (interfaces != null)
            {
                var _interfaces = interfaces.Select(s => SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(s)));

                baseList = baseList.AddTypes(_interfaces.ToArray());
            }

            // Add inherited type and implemented interfaces
            clsDec = clsDec.WithBaseList(baseList);

            // TODO: add attributes
            // TODO: add generics

            return clsDec;
        }

        /// <summary>
        /// Creates an InterfaceDeclaration node with a specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static InterfaceDeclarationSyntax Interface(string name)
        {
            return SyntaxFactory.InterfaceDeclaration(name)
                .WithModifiers(
                    SyntaxFactory.TokenList(new SyntaxToken[] { SyntaxKind.PublicKeyword.ToToken() }));
        }

        public static SyntaxNode Enum(string name, BaseListSyntax baseClass = null)
        {
            return SyntaxFactory.EnumDeclaration(
                SyntaxFactory.List<AttributeListSyntax>(),
                SyntaxFactory.TokenList(),
                SyntaxFactory.ParseToken(name),
                baseClass,
                SyntaxFactory.SeparatedList<EnumMemberDeclarationSyntax>());
        }

        public static SyntaxNode Struct(string name)
        {
            return SyntaxFactory.StructDeclaration(name);
        }

        public static DelegateDeclarationSyntax Delegate(string name, TypeSyntax type = null)
        {
            type = type ?? SyntaxFactory.PredefinedType(SyntaxKind.VoidKeyword.ToToken());

            return SyntaxFactory.DelegateDeclaration(type, name);
        }
    }
}