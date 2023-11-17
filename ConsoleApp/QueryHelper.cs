﻿using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;

namespace ConsoleApp;

public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) =>
        deliveries.Where((delivery) => delivery.PaymentId != null); //TODO: Завдання 1

    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
    public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries) =>
        deliveries.Where((delivery) =>
            delivery.Status != DeliveryStatus.Cancelled && delivery.Status != DeliveryStatus.Done); //TODO: Завдання 2

    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    public IEnumerable<DeliveryShortInfo> DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) =>
        deliveries.Where((delivery) => delivery.ClientId == clientId).Select((delivery) => new DeliveryShortInfo()
        {
            ClientId = clientId,
            Id = delivery.Id,
            Type = delivery.Type,
            Status = delivery.Status,
            ArrivalPeriod = delivery.ArrivalPeriod,
            CargoType = delivery.CargoType,
            StartCity = delivery.Direction.Origin.City,
            EndCity = delivery.Direction.Destination.City,
            LoadingPeriod = delivery.LoadingPeriod,
        }); //TODO: Завдання 3

    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have specified type
    /// </summary>
    public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery> deliveries, string cityName,
        DeliveryType type) =>
        deliveries.Where((delivery) => delivery.Direction.Origin.City == cityName && delivery.Type == type)
            .Take(10); //TODO: Завдання 4

    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    public IEnumerable<Delivery> OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) =>
        deliveries.OrderBy((delivery) => delivery.Status)
            .ThenBy((delivery) => delivery.LoadingPeriod.Start); //TODO: Завдання 5

    /// <summary>
    /// Count unique cargo types
    /// </summary>
    public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) =>
        deliveries.DistinctBy((delivery) => delivery.CargoType).Count(); //TODO: Завдання 6

    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    public Dictionary<DeliveryStatus, int> CountsByDeliveryStatus(IEnumerable<Delivery> deliveries) =>
        deliveries.GroupBy((delivery) => delivery.Status)
            .ToDictionary((iGroup) => iGroup.Key, (iGroup) => iGroup.Count()); //TODO: Завдання 7

    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap between end of loading period and start of arrival period (calculate in minutes)
    /// </summary>
    public IEnumerable<AverageGapsInfo> AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries) =>
        deliveries.GroupBy((delivery) => delivery.Direction).Select((iGroup) => new AverageGapsInfo()
        {
            AverageGap = iGroup.Average((delivery) =>
                (delivery.ArrivalPeriod.Start.Value - delivery.LoadingPeriod.End.Value).Minutes),
            StartCity = iGroup.Key.Origin.City,
            EndCity = iGroup.Key.Destination.City,
        }); //TODO: Завдання 8

    /// <summary>
    /// Paging helper
    /// </summary>
    public IEnumerable<TElement> Paging<TElement, TOrderingKey>(IEnumerable<TElement> elements,
        Func<TElement, TOrderingKey> ordering,
        Func<TElement, bool>? filter = null,
        int countOnPage = 100,
        int pageNumber = 1) =>
        elements.OrderBy(ordering).Where(filter ?? ((t) => true)).Skip((pageNumber - 1) * countOnPage)
            .Take(countOnPage); //TODO: Завдання 9 
}