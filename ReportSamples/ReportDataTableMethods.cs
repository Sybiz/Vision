public void AddDataTable(string dataTableName, string primaryKey, Type primaryKeyType, string query);

public void AddDataTable(string dataTableName, string primaryKey, Type primaryKeyType, string query, Dictionary<string, object> paramaters);

public void AddDataTable(string dataTableName, string relationshipName, string primaryKey, Type primaryKeyType, string parentTable, string parentKey, string query, Dictionary<string, object> paramaters);

public void AddDataTable(string dataTableName, string relationshipName, string relationshipKey, Type relationshipKeyType, string primaryKey, Type primaryKeyType, string parentTable, string parentKey, string query, Dictionary<string, object> paramaters);
