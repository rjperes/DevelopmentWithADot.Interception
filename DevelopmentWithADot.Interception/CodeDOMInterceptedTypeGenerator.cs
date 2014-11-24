﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace DevelopmentWithADot.Interception
{
	internal class CodeDOMInterceptedTypeGenerator : InterceptedTypeGenerator
	{
		private static readonly CodeDomProvider provider = new CSharpCodeProvider();
		private static readonly CodeGeneratorOptions options = new CodeGeneratorOptions() { BracingStyle = "C" };
		private static readonly CodeTypeReference proxyTypeReference = new CodeTypeReference(typeof(IInterceptionProxy));
		private static readonly CodeTypeReference interceptorTypeReference = new CodeTypeReference(typeof(Interceptor));
		private static readonly CodeTypeReference handlerTypeReference = new CodeTypeReference(typeof(IInterceptionHandler));
		private static readonly Assembly proxyAssembly = typeof(IInterceptionProxy).Assembly;
		private static readonly Type interfaceProxyType = typeof(InterfaceProxy);

		protected virtual void GenerateConstructors(CodeTypeDeclaration targetClass, Type baseType, IEnumerable<ConstructorInfo> constructors)
		{
			foreach (ConstructorInfo constructor in constructors)
			{
				CodeConstructor c = new CodeConstructor();
				targetClass.Members.Add(c);

				c.Attributes = MemberAttributes.Final;

				foreach (ParameterInfo parameter in constructor.GetParameters())
				{
					c.Parameters.Add(new CodeParameterDeclarationExpression(parameter.ParameterType, parameter.Name));
				}

				if (baseType == interfaceProxyType)
				{
					c.Attributes |= MemberAttributes.Public;
				}
				else
				{
					if ((constructor.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
					{
						c.Attributes |= MemberAttributes.Public;
					}
					else if ((constructor.Attributes & MethodAttributes.FamORAssem) == MethodAttributes.FamORAssem)
					{
						c.Attributes |= MemberAttributes.FamilyOrAssembly;
					}
					else if ((constructor.Attributes & MethodAttributes.Family) == MethodAttributes.Family)
					{
						c.Attributes |= MemberAttributes.Family;
					}
					else if ((constructor.Attributes & MethodAttributes.FamANDAssem) == MethodAttributes.FamANDAssem)
					{
						c.Attributes |= MemberAttributes.FamilyAndAssembly;
					}
				}

				foreach (ParameterInfo p in constructor.GetParameters())
				{
					c.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression(p.Name));
				}
			}
		}

		protected virtual void GenerateMethods(CodeTypeDeclaration targetClass, Type baseType, IEnumerable<MethodInfo> methods)
		{
			if (methods.Any() == true)
			{
				MethodInfo finalizeMethod = methods.First().DeclaringType.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance);

				foreach (MethodInfo method in methods)
				{
					if (method == finalizeMethod)
					{
						continue;
					}

					if (method.IsSpecialName == true)
					{
						continue;
					}

					CodeMemberMethod m = new CodeMemberMethod();
					m.Name = method.Name;
					m.ReturnType = new CodeTypeReference(method.ReturnType);

					if (baseType != interfaceProxyType)
					{
						m.Attributes = MemberAttributes.Override;
					}

					targetClass.Members.Add(m);

					foreach (ParameterInfo parameter in method.GetParameters())
					{
						m.Parameters.Add(new CodeParameterDeclarationExpression(parameter.ParameterType, parameter.Name));
					}

					if ((method.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
					{
						m.Attributes |= MemberAttributes.Public;
					}
					else if ((method.Attributes & MethodAttributes.FamORAssem) == MethodAttributes.FamORAssem)
					{
						m.Attributes |= MemberAttributes.FamilyOrAssembly;
					}
					else if ((method.Attributes & MethodAttributes.Family) == MethodAttributes.Family)
					{
						m.Attributes |= MemberAttributes.Family;
					}
					else if ((method.Attributes & MethodAttributes.FamANDAssem) == MethodAttributes.FamANDAssem)
					{
						m.Attributes |= MemberAttributes.FamilyAndAssembly;
					}

					if (baseType == interfaceProxyType)
					{
						m.Attributes = MemberAttributes.Public | MemberAttributes.Final;
					}

					CodeExpression[] ps = new CodeExpression[] { new CodeThisReferenceExpression(), new CodeCastExpression(typeof(MethodInfo), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(MethodBase)), "GetCurrentMethod"))) }.Concat(method.GetParameters().Select(x => new CodeVariableReferenceExpression(x.Name))).ToArray();
					CodeVariableDeclarationStatement arg = new CodeVariableDeclarationStatement(typeof(InterceptionArgs), "args", new CodeObjectCreateExpression(typeof(InterceptionArgs), ps));

					m.Statements.Add(arg);

					CodeMethodInvokeExpression handler = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "handler"), "Invoke"), new CodeVariableReferenceExpression("args"));
					m.Statements.Add(handler);

					CodeBinaryOperatorExpression comparison = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("args"), "Handled"), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(method.ReturnType != typeof(void)));
					CodeConditionStatement @if = null;

					if (method.ReturnType != typeof(void))
					{
						if (baseType != interfaceProxyType)
						{
							@if = new CodeConditionStatement(comparison, new CodeMethodReturnStatement(new CodeCastExpression(method.ReturnType, new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("args"), "Result"))));

							m.Statements.Add(@if);
							m.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), method.Name, method.GetParameters().Select(x => new CodeArgumentReferenceExpression(x.Name)).ToArray())));
						}
						else
						{
							@if = new CodeConditionStatement(comparison, new CodeMethodReturnStatement(new CodeCastExpression(method.ReturnType, new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("args"), "Result"))));

							m.Statements.Add(@if);
							m.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeCastExpression(method.DeclaringType, new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "instance")), m.Name), method.GetParameters().Select(x => new CodeArgumentReferenceExpression(x.Name)).ToArray())));
						}
					}
					else
					{
						if (baseType != interfaceProxyType)
						{
							@if = new CodeConditionStatement(comparison, new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), method.Name, method.GetParameters().Select(x => new CodeArgumentReferenceExpression(x.Name)).ToArray())));

							m.Statements.Add(@if);
						}
						else
						{
							@if = new CodeConditionStatement(comparison, new CodeMethodReturnStatement(new CodeCastExpression(method.ReturnType, new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("args"), "Result"))));

							m.Statements.Add(@if);
							m.Statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeCastExpression(method.DeclaringType, new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "instance")), m.Name), method.GetParameters().Select(x => new CodeArgumentReferenceExpression(x.Name)).ToArray())));
						}
					}
				}
			}
		}

		protected virtual void GenerateProperties(CodeTypeDeclaration targetClass, Type baseType, IEnumerable<PropertyInfo> properties)
		{
			CodeMemberProperty interceptorProperty = new CodeMemberProperty();
			interceptorProperty.Name = "Interceptor";
			interceptorProperty.Type = interceptorTypeReference;
			interceptorProperty.PrivateImplementationType = proxyTypeReference;
			interceptorProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "interceptor")));
			targetClass.Members.Add(interceptorProperty);

			foreach (PropertyInfo property in properties)
			{
				CodeMemberProperty p = new CodeMemberProperty();
				p.Name = property.Name;
				p.Type = new CodeTypeReference(property.PropertyType);

				if (baseType != interfaceProxyType)
				{
					p.Attributes = MemberAttributes.Override;
				}

				targetClass.Members.Add(p);

				if (property.CanRead == true)
				{
					p.HasGet = true;

					if ((property.GetMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
					{
						p.Attributes |= MemberAttributes.Public;
					}
					else if ((property.GetMethod.Attributes & MethodAttributes.FamORAssem) == MethodAttributes.FamORAssem)
					{
						p.Attributes |= MemberAttributes.FamilyOrAssembly;
					}
					else if ((property.GetMethod.Attributes & MethodAttributes.Family) == MethodAttributes.Family)
					{
						p.Attributes |= MemberAttributes.Family;
					}
					else if ((property.GetMethod.Attributes & MethodAttributes.FamANDAssem) == MethodAttributes.FamANDAssem)
					{
						p.Attributes |= MemberAttributes.FamilyAndAssembly;
					}

					if (baseType == interfaceProxyType)
					{
						p.Attributes = MemberAttributes.Public | MemberAttributes.Final;
					}

					CodeExpression[] ps = new CodeExpression[]
					{
						new CodeThisReferenceExpression(),
						new CodeCastExpression(typeof(MethodInfo), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(MethodBase)), "GetCurrentMethod")))
					};
					
					CodeVariableDeclarationStatement arg = new CodeVariableDeclarationStatement(typeof(InterceptionArgs), "args", new CodeObjectCreateExpression(typeof(InterceptionArgs), ps));

					p.GetStatements.Add(arg);

					CodeMethodInvokeExpression handler = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "handler"), "Invoke"), new CodeVariableReferenceExpression("args"));
					p.GetStatements.Add(handler);

					CodeBinaryOperatorExpression comparison = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("args"), "Handled"), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(true));
					CodeConditionStatement @if = new CodeConditionStatement(comparison, new CodeMethodReturnStatement(new CodeCastExpression(property.PropertyType, new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("args"), "Result"))));

					p.GetStatements.Add(@if);

					if (baseType != interfaceProxyType)
					{
						p.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), property.Name)));
					}
					else
					{
						p.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeCastExpression(property.DeclaringType, new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "instance")), property.Name)));
					}
				}

				if (property.CanWrite == true)
				{
					p.HasSet = true;

					CodeExpression[] ps = new CodeExpression[]
					{
						new CodeThisReferenceExpression(),
						new CodeCastExpression(typeof(MethodInfo), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(MethodBase)), "GetCurrentMethod"))),
						new CodeVariableReferenceExpression("value")
					};

					CodeVariableDeclarationStatement arg = new CodeVariableDeclarationStatement(typeof(InterceptionArgs), "args", new CodeObjectCreateExpression(typeof(InterceptionArgs), ps));

					p.SetStatements.Add(arg);

					CodeMethodInvokeExpression handler = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "handler"), "Invoke"), new CodeVariableReferenceExpression("args"));
					p.SetStatements.Add(handler);

					CodeBinaryOperatorExpression comparison = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("args"), "Handled"), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(true));
					CodeConditionStatement @if = new CodeConditionStatement(comparison, new CodeMethodReturnStatement());

					p.SetStatements.Add(@if);

					if (baseType != interfaceProxyType)
					{
						p.SetStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), property.Name), new CodeVariableReferenceExpression("value")));
					}
					else
					{
						p.SetStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeCastExpression(property.DeclaringType, new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "instance")), property.Name), new CodeVariableReferenceExpression("value")));
					}
				}
			}
		}

		protected virtual void GenerateFields(CodeTypeDeclaration targetClass, Type baseType, Type handlerType, Type interceptorType)
		{
			if (handlerType != null)
			{
				CodeMemberField handlerField = new CodeMemberField();
				handlerField.Attributes = MemberAttributes.FamilyOrAssembly;
				handlerField.Name = "handler";
				handlerField.Type = handlerTypeReference;
				handlerField.InitExpression = handlerType != null ? new CodeObjectCreateExpression(handlerType) : null;
				targetClass.Members.Add(handlerField);
			}

			CodeMemberField interceptorField = new CodeMemberField();
			interceptorField.Attributes = MemberAttributes.FamilyOrAssembly;
			interceptorField.Name = "interceptor";
			interceptorField.Type = interceptorTypeReference;
			interceptorField.InitExpression = interceptorType != null ? new CodeObjectCreateExpression(interceptorType) : null;
			targetClass.Members.Add(interceptorField);
		}

		protected virtual void AddReferences(CompilerParameters parameters, Type baseType, Type handlerType, Type interceptorType, Type [] additionalInterfaceTypes)
		{
			parameters.ReferencedAssemblies.Add(String.Concat(proxyAssembly.GetName().Name, Path.GetExtension(proxyAssembly.CodeBase)));
			parameters.ReferencedAssemblies.Add(String.Concat(baseType.Assembly.GetName().Name, Path.GetExtension(baseType.Assembly.CodeBase)));

			if (handlerType != null)
			{
				parameters.ReferencedAssemblies.Add(String.Concat(handlerType.Assembly.GetName().Name, Path.GetExtension(handlerType.Assembly.CodeBase)));
			}

			if (interceptorType != null)
			{
				parameters.ReferencedAssemblies.Add(String.Concat(interceptorType.Assembly.GetName().Name, Path.GetExtension(interceptorType.Assembly.CodeBase)));
			}

			foreach (Type additionalInterfaceType in additionalInterfaceTypes)
			{
				parameters.ReferencedAssemblies.Add(String.Concat(additionalInterfaceType.Assembly.GetName().Name, Path.GetExtension(additionalInterfaceType.Assembly.CodeBase)));
			}
		}

		protected virtual IEnumerable<PropertyInfo> GetProperties(Type baseType, Type[] additionalInterfaceTypes)
		{
			if (baseType != interfaceProxyType)
			{
				return (baseType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => ((x.CanRead == true) && (x.GetMethod.IsVirtual == true)) || ((x.CanWrite == true) && (x.SetMethod.IsVirtual == true))).Concat(additionalInterfaceTypes.SelectMany(x => x.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(y => ((y.CanRead == true) && (y.GetMethod.IsVirtual == true)) || ((y.CanWrite == true) && (y.SetMethod.IsVirtual == true))))).Distinct().ToList());
			}
			else
			{
				return (additionalInterfaceTypes.SelectMany(x => x.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(y => ((y.CanRead == true) && (y.GetMethod.IsVirtual == true)) || ((y.CanWrite == true) && (y.SetMethod.IsVirtual == true)))).Distinct().ToList());
			}
		}

		protected virtual IEnumerable<MethodInfo> GetMethods(Type baseType, Type [] additionalInterfaceTypes)
		{
			if (baseType != interfaceProxyType)
			{
				return (baseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.IsVirtual == true).Concat(additionalInterfaceTypes.SelectMany(x => x.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(y => y.IsVirtual == true))).Distinct().ToList());
			}
			else
			{
				return (additionalInterfaceTypes.SelectMany(x => x.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(y => y.IsVirtual == true)).Distinct().ToList());
			}
		}

		protected virtual IEnumerable<ConstructorInfo> GetConstructors(Type baseType)
		{
			return (baseType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
		}

		public override Type Generate(Interceptor interceptor, Type baseType, Type handlerType, params Type[] additionalInterfaceTypes)
		{
			IEnumerable<PropertyInfo> properties = this.GetProperties(baseType, additionalInterfaceTypes);
			IEnumerable<MethodInfo> methods = this.GetMethods(baseType, additionalInterfaceTypes);
			IEnumerable<ConstructorInfo> constructors = this.GetConstructors(baseType);

			CodeTypeDeclaration targetClass = new CodeTypeDeclaration(String.Concat(baseType.Name, "_Dynamic"));
			targetClass.IsClass = baseType.IsClass;
			targetClass.TypeAttributes = TypeAttributes.Sealed | TypeAttributes.Serializable;
			targetClass.BaseTypes.Add((baseType.IsInterface == false) ? baseType : typeof(Object));
			targetClass.BaseTypes.Add(proxyTypeReference.BaseType);

			foreach (Type additionalInterfaceType in additionalInterfaceTypes)
			{
				targetClass.BaseTypes.Add(additionalInterfaceType);
			}

			CodeNamespace samples = new CodeNamespace(baseType.Namespace);
			samples.Imports.Add(new CodeNamespaceImport(typeof(String).Namespace));
			samples.Types.Add(targetClass);

			CodeCompileUnit targetUnit = new CodeCompileUnit();
			targetUnit.Namespaces.Add(samples);

			this.GenerateFields(targetClass, baseType, handlerType, interceptor != null ? interceptor.GetType() : null);

			this.GenerateConstructors(targetClass, baseType, constructors);

			this.GenerateMethods(targetClass, baseType, methods);

			this.GenerateProperties(targetClass, baseType, properties);

			StringBuilder builder = new StringBuilder();
			
			using (TextWriter sourceWriter = new StringWriter(builder))
			{
				provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
			}

			CompilerParameters parameters = new CompilerParameters() { GenerateInMemory = true };

			this.AddReferences(parameters, baseType, handlerType, interceptor != null ? interceptor.GetType() : null, additionalInterfaceTypes);
			
			CompilerResults results = provider.CompileAssemblyFromDom(parameters, targetUnit);

			return (results.CompiledAssembly.GetTypes().First());
		}
	}
}
