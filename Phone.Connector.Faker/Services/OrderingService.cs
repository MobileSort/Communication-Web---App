using Phone.Connector.Faker.Models.ViewModel;

namespace Phone.Connector.Faker.Utils;

public class OrderingService
{
  public OrderingService()
  {
  }

  public List<Ordering> ListOrderings()
  {
    InternalConfig config = InternalConfigSingle.internalConfigRead;
    return config.Orderings;
  }

  public Ordering? GetOrderingById(int id)
  {
    var orderings = ListOrderings();
    if (orderings.Count == 0)
    {
      return null;
    }

    Ordering? foundOrdering = orderings.Find((ordering) => ordering.IdOrdering == id);
    return foundOrdering;
  }

  public int? AddOrdering(string Name, List<int> TagIds, string DirectoryDestination)
  {
    if (TagIds.Count > 0)
    {
      TagService tagService = new();
      if (TagIds.Any((tagId) =>
              tagService.GetTagById(tagId) == null
          ))
      {
        return null;
      }
    }

    int lastId = 0;
    foreach (var ordering in InternalConfigSingle.internalConfigRead.Orderings)
    {
      if (ordering.IdOrdering > lastId)
      {
        lastId = ordering.IdOrdering;
      }
    }

    Ordering orderingToAdd = new(lastId + 1, Name, TagIds, DirectoryDestination);
    InternalConfigSingle.internalConfigRead.Orderings.Add(orderingToAdd);
    return orderingToAdd.IdOrdering;
  }

  public bool RemoveOrdering(int id)
  {
    var foundOrdering = GetOrderingById(id);
    if (foundOrdering == null)
    {
      return false;
    }

    return InternalConfigSingle.internalConfigRead.Orderings.Remove(foundOrdering);
  }

  public bool ExecuteOrdering(int IdOrdering, string targetDirectoryPath)
  {
    var ordering = GetOrderingById(IdOrdering);
    if (ordering == null)
    {
      return false;
    }
    DirectoryService directoryService = new DirectoryService(Constants.StoragePath);

    List<Tag> orderingTags = [];
    foreach (var tagId in ordering.TagIds)
    {
      TagService tagService = new TagService();
      Tag? tag = tagService.GetTagById(tagId);
      if (tag == null)
      {
        return false;
      }
      orderingTags.Add(tag);
    }
    if (!directoryService.MoveItemsOnDirectoryByTags(targetDirectoryPath, orderingTags, ordering.DirectoryDestination))
    {
      return false;
    }
    directoryService.WriteChanges();
    return true;
  }



  public bool WriteChanges()
  {
    return InternalConfigSingle.WriteChanges();
  }
}