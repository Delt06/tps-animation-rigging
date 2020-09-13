using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Validation.Editor
{
	public static class AutoValidator
	{
		[MenuItem("Dependencies/Validate"), InitializeOnEnterPlayMode]
		public static void Run()
		{
			var objects = Object.FindObjectsOfType<Component>();

			var invalid = new List<Component>();

			foreach (var context in objects)
			{
				var type = context.GetType();
				var fields = type.GetDependencyFields();

				foreach (var field in fields)
				{
					var attribute = field.GetCustomAttribute<DependencyAttribute>();

					if (!ComponentIsAttached(context, attribute.Source, field.FieldType))
						invalid.Add(context);
				}

				foreach (var property in type.GetDependencyProperties())
				{
					var attribute = property.GetCustomAttribute<DependencyAttribute>();

					if (!property.CanWrite)
						throw new InvalidOperationException($"Property {property} of {type} has no setter.");

					if (!ComponentIsAttached(context, attribute.Source, property.PropertyType))
						invalid.Add(context);
				}
			}

			var uniqueInvalid = invalid
				.Distinct()
				.ToList();

			if (uniqueInvalid.Count == 0)
			{
				Debug.Log("Validation passed successfully.");
			}
			else
			{
				Debug.LogError($"Validation failed. {uniqueInvalid.Count} invalid component(s) were detected. Among them:");

				for (var index = 0; index < uniqueInvalid.Count && index < PrintedComponentsCount; index++)
				{
					var component = uniqueInvalid[index];
					Debug.LogError(component, component);
				}
				
				if (uniqueInvalid.Count > PrintedComponentsCount)
					Debug.LogError($"And {uniqueInvalid.Count - PrintedComponentsCount} more...");
			}
		}

		private static bool ComponentIsAttached(Component context, Source source, Type type)
		{
			switch (source)
			{
				case Source.Local: return context.GetComponent(type) != null;
				case Source.FromParents: return context.GetComponentInParent(type) != null;
				case Source.FromChildren: return context.GetComponentsInChildren(type) != null;
				case Source.Global
					when typeof(Object).IsAssignableFrom(type): return Object.FindObjectsOfType(type) != null;
				case Source.Global: throw ValidationExtensions.NewGlobalDependencyIllegalTypeException(type, context);
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private const int PrintedComponentsCount = 3;
	}
}