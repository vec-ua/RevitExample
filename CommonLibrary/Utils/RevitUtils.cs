using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;

namespace CommonLibrary.Utils
{
    /// <summary>
    /// Класс утилита для работы с общими данными
    /// </summary>
    public static class RevitUtils
    {
        /// <summary>
        /// Получить список элементов по идентификатору
        /// </summary>
        /// <param name="document">Объект, представляющий открытый проект Autodesk Revit</param>
        /// <param name="elementIds">Список объектов ElementId, используется как уникальный идентификатор для элемента в рамках одного проекта</param>
        /// <returns>Список элементов</returns>
        public static IEnumerable<Element> GetElementsFromIds(Document document, ICollection<ElementId> elementIds)
        {
            List<Element> elements = new List<Element>();

            foreach (ElementId id in elementIds)
            {
                Element element = document.GetElement(id);
                if (element == null)
                    continue;

                elements.Add(element);
            }

            return elements;
        }

        /// <summary>
        /// Получить все выбраные объекты указаного типа
        /// </summary>
        /// <param name="document">Объект, представляющий открытый проект Autodesk Revit</param>
        /// <param name="selection">Текущий пользовательский выбор элементов в проекте</param>
        /// <returns>Список объекты указаного типа</returns>
        public static IEnumerable<T> GetSelectedConduits<T>(Document document, Selection selection)
        {
            ICollection<ElementId> selectedElementsId = selection.GetElementIds();
            IEnumerable<Element> selectedElements = GetElementsFromIds(document, selectedElementsId);

            List<T> conduits = new List<T>();
            foreach (Element element in selectedElements)
            {
                if (element is T conduit)
                    conduits.Add(conduit);
            }

            return conduits;
        }
    }
}
