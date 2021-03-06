﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace KnightTourAlg
{
    class Handler
    {
        private readonly MoveListSwitcher switcher;

        private readonly int width;
        private readonly int height;

        // Флаг реверса
        private bool reverse = false;

        // Отмечаем пройденные
        private bool[,] passed;

        // Текущий ход
        private int step;
        private Coords start;
        private Coords end;

        // Для обхода цикличности при итерации
        private Coords? lastCancelled;

        public Handler(Coords start, Coords end) : this(8, start, end) { }

        public Handler(int size, Coords start, Coords end) : this(size, size, start, end) { }

        public Handler(int width, int height, Coords start, Coords end)
        {
            if (width <= 2 || height <= 2)
            {
                throw new ArgumentException("Widht/height cannot be <= 2");
            }

            if (start.X < 0 || start.Y < 0 || start.X >= width || start.Y >= height)
            {
                throw new ArgumentException("Start position not on board");
            }

            if (end.X < 0 || end.Y < 0 || end.X >= width || end.Y >= height)
            {
                throw new ArgumentException("Start position not on board");
            }

            this.width = width;
            this.height = height;
            this.start = start;
            this.end = end;
            step = 0;
            switcher = new MoveListSwitcher();
            CleanBoard();
        }

        private void CleanBoard()
        {
            lastCancelled = null;
            passed = new bool[width, height];
        }

        // Делаем базовые проверки и ищем путь
        public bool Execute(out List<Coords> path)
        {
            // Стартовая клетка - начало пути
            path = new List<Coords> {start};
            passed[start.X, start.Y] = true;

            // Проверяем по цветам клеток существование пути
            if (!SolutionPossible())
            {
                return false;
            }

            // Идем до последнего шага
            for (step = 1; step < width * height;)
            {
                // Пробуем совершить доступный ход
                var move = MakeMoveFrom(path.Last());
                if (move.HasValue && !BlockedExist(move.Value))
                {
                    step++;
                    path.Add(move.Value);
                    passed[move.Value.X, move.Value.Y] = true;
                    lastCancelled = null;
                }
                else
                {
                    // Откатываемся если не получилось
                    if (step == 1)
                    {
                        // Фейл
                        break;
                    }

                    // Дописываем откат
                    switcher.CurrentReturn++;
                    if (switcher.CurrentReturn > switcher.MaxReturn)
                    {
                        // Меняем сеты/увеличиваем циклы
                        switcher.CurrentReturn = 0;
                        switcher.SwitchSet();
                        // ~10000 - порог для реверсирования
                        if (switcher.MaxReturn > 10000)
                        {
                            var temp = end;
                            end = start;
                            start = temp;
                            // Скидываем количество циклов!!
                            switcher.MaxReturn = 1;
                            // Флаг реверса для корректного вывода!!
                            reverse = true;
                        }
                        // Обнуляем прогресс
                        step = 1;
                        path = new List<Coords> {start};
                        CleanBoard();
                        passed[start.X, start.Y] = true;
                        // Перезапускаемся
                        continue;
                    }

                    step--;
                    var last = path.Last();
                    passed[last.X, last.Y] = false;
                    lastCancelled = last;
                    path.Remove(last);
                }
            }

            if (reverse)
            {
                path.Reverse();
            }
            return step == width * height;
        }

        // Оптимизация 3 - Проверка блокированных ячеек
        private bool BlockedExist(Coords move)
        {
            // Игнорируем предпоследний ход и далее
            if (step >= width * height - 3)
            {
                return false;
            }

            // Проверяем все лежащие рядом ячейки на возникновение блокировки (0 ходов)
            var all = FindAvailableMoves(move, step);
            foreach (var coords in all)
            {
                if (FindAvailableMoves(coords, step).Count == 0)
                {
                    return true;
                }
            }

            return false;
        }

        // Оптимизация 1 - исключение заведомо несуществующих путей
        private bool SolutionPossible()
        {
            // Петля невозможна по условию
            if (start == end)
            {
                return false;
            }

            // Для четной доски конечная и начальная клетки разных цветов
            if (width * height % 2 == 0 && (start.X + start.Y) % 2 != (end.X + end.Y) % 2)
            {
                return true;
            }

            // Для нечетной доски стартовая и конечная клетка черные
            if (width * height % 2 != 0 && (start.X + start.Y) % 2 == 0 && (end.X + end.Y) % 2 == 0)
            {
                return true;
            }

            return false;
        }

        // Совершение хода из клетки
        private Coords? MakeMoveFrom(Coords from)
        {
            // Получаем оптимизированный список
            var availableMoves = SortVarnsdorf(FindAvailableMoves(from, step));
            // Неудача если нет ходов
            if (availableMoves.Count == 0)
            {
                return null;
            }

            // Смещаемся при повторных проходах
            if (!lastCancelled.HasValue)
            {
                return availableMoves[0];
            }

            // Смещение пропускает уже пройденные ходы
            var index = availableMoves.Count;
            for (var i = 0; i < availableMoves.Count; i++)
            {
                if (availableMoves[i] != lastCancelled.Value)
                {
                    continue;
                }

                index = i + 1;
                break;
            }

            // Если смещение максимально - неудача
            if (index >= availableMoves.Count)
            {
                return null;
            }

            return availableMoves[index];
        }

        // Оптимизация 2 - Метод Варнсдорфа
        private List<Coords> SortVarnsdorf(IReadOnlyCollection<Coords> input)
        {
            var result = input;
            // Получаем доступные для текущего состояния ходы и сортируем в соотвествии с приоритетом
            // Приоритет клетки определяется количеством доступных ходов из неё
            return new List<Coords>(result.OrderBy(x => FindAvailableMoves(x, step).Count));
        }

        // Выборка доступных ходов для текущего состояния
        private List<Coords> FindAvailableMoves(Coords from, int currentStep)
        {
            var available = new List<Coords>();

            foreach (var move in switcher.Sets[switcher.CurrentSet])
            {
                var target = move + from;

                // Выбрасываем те что вне доски 
                if (target.X < 0 || target.X >= width)
                {
                    continue;
                }

                if (target.Y < 0 || target.Y >= height)
                {
                    continue;
                }

                // Выбрасываем пройденные
                if (passed[target.X, target.Y])
                {
                    continue;
                }

                // Игнорируем конечную клетку до последнего хода
                if (currentStep != width * height - 1 && target == end)
                {
                    continue;
                }

                // Игнорируем все клетки кроме конечной на последнем ходу
                if (currentStep == width * height - 1 && target != end)
                {
                    continue;
                }

                available.Add(target);
            }

            return available;
        }
    }
}