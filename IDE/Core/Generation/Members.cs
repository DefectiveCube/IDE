using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core.Generation
{
    public static partial class Generator
    {
        public static EventDeclarationSyntax Event(string name, TypeSyntax type)
        {
            return SyntaxFactory.EventDeclaration(type, name);
        }

        public static FieldDeclarationSyntax Field(string name, TypeSyntax type, SyntaxList<AttributeListSyntax> attributes = default(SyntaxList<AttributeListSyntax>), SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return SyntaxFactory.FieldDeclaration(attributes, modifiers, Variable(type, name));
        }

        public static AccessorDeclarationSyntax Getter(MemberAccessModifier access = MemberAccessModifier.Unspecified, BlockSyntax block = null)
        {
            return Accessor(SyntaxKind.GetAccessorDeclaration, SyntaxFactory.Token(SyntaxKind.GetKeyword), block);

            // TODO: adjust access modifier (if present)
        }

        public static AccessorDeclarationSyntax Setter(MemberAccessModifier access = MemberAccessModifier.Unspecified, BlockSyntax block = null)
        {
            return Accessor(SyntaxKind.SetAccessorDeclaration, SyntaxFactory.Token(SyntaxKind.SetKeyword), block);

            // TODO: adjust access modifier (if present)
        }

        public static AccessorDeclarationSyntax GetterAuto(MemberAccessModifier access = MemberAccessModifier.Unspecified)
        {
            throw new NotImplementedException();
            //return Accessor
        }

        internal static AccessorDeclarationSyntax Accessor(SyntaxKind kind, SyntaxToken keyword, BlockSyntax block = null)
        {
            return block != null ?
                SyntaxFactory.AccessorDeclaration(kind, block) :
                SyntaxFactory.AccessorDeclaration(kind, default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), keyword, block, SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }

        public static PropertyDeclarationSyntax Property(
            string name,
            TypeSyntax type,
            TypeAccessModifier access = TypeAccessModifier.Unspecified,
            AccessorDeclarationSyntax getter = null,
            AccessorDeclarationSyntax setter = null
        )
        {
            var list = SyntaxFactory.AccessorList();

            if (getter != null)
            {
                list = list.AddAccessors(new[] { getter });
            }

            if (setter != null)
            {
                list = list.AddAccessors(new[] { setter });
            }

            return SyntaxFactory.PropertyDeclaration(type, name)
                    .WithAccessorList(list);
        }

        public static ConstructorDeclarationSyntax Constructor(string name, MemberAccessModifier access = MemberAccessModifier.Unspecified, bool isStatic = false)
        {
            // Create a ctor declaration with an empty expression body
            var dec = SyntaxFactory.ConstructorDeclaration(name)
                .WithBody(Block());

            switch (access)
            {
                case MemberAccessModifier.Private:
                    dec = dec.WithModifiers(SyntaxFactory.TokenList(SyntaxKind.PrivateKeyword.ToToken()));
                    break;
                case MemberAccessModifier.Protected:
                    dec = dec.WithModifiers(SyntaxFactory.TokenList(SyntaxKind.ProtectedKeyword.ToToken()));
                    break;
                case MemberAccessModifier.Public:
                    dec = dec.WithModifiers(SyntaxFactory.TokenList(SyntaxKind.PublicKeyword.ToToken()));
                    break;
                default:
                    break;
            }

            return dec;
        }

        public static DestructorDeclarationSyntax Destructor(string name, BlockSyntax body = null)
        {
            return body == null ?
                SyntaxFactory.DestructorDeclaration(name).WithBody(Block()) :
                SyntaxFactory.DestructorDeclaration(name).WithBody(body);
        }

        public static MethodDeclarationSyntax Method(
            string name,
            TypeSyntax returnType = null,
            MemberAccessModifier access = MemberAccessModifier.Unspecified,
            bool isAbstract = false,
            bool isExtension = false,
            bool isOverrides = false,
            bool isStatic = false,
            bool isVirtual = false,
            ParameterListSyntax parameters = null,
            BlockSyntax body = null
        )
        {
            // If returnType is null, set it to "void" type token
            returnType = returnType ?? SyntaxKind.VoidKeyword.ToTypeSyntax();

            // Instantiate method declaration
            var dec = SyntaxFactory.MethodDeclaration(returnType, name);

            // If method body is not passed, use an empty block
            dec = body != null ? dec.WithBody(body) : dec.WithBody(Block());

            // Add abstract keyword token to declaration
            if (isAbstract)
            {
                dec = dec.AddModifiers(new[] { SyntaxKind.AbstractKeyword.ToToken() });
            }

            // Add overrides keyword token to declaration
            if (isOverrides)
            {
                dec = dec.AddModifiers(new[] { SyntaxKind.OverrideKeyword.ToToken() });
            }

            if (isStatic || isExtension)
            {
                dec = dec.AddModifiers(new[] { SyntaxKind.StaticKeyword.ToToken() });
            }

            if (isVirtual)
            {
                dec = dec.AddModifiers(new[] { SyntaxKind.VirtualKeyword.ToToken() });
            }

            //if (isExtension)
            //{
                // TODO: ensure the first parameter has "this"                
            //}

            dec = parameters != null ? dec.WithParameterList(parameters) : dec;

            return dec;
        }
    }
}
