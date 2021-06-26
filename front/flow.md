``` mermaid
sequenceDiagram
    Client->>Server: アクセス要求
    Server->>AzurePubSubService: SDKを介して接続情報など作成
    AzurePubSubService-->>Server: アクセストークン返却
    Server-->>Client: アクセストークン付与したwssURLを返却
    Client->>AzurePubSubService: wssURLを使ってOpen
    activate Client
    AzurePubSubService->>AzureFunctions: Connect Event
    AzurePubSubService-->>Client: 接続確立
    AzurePubSubService->>AzureFunctions: Connected Event
    Client->>AzurePubSubService: メッセージをぶん投げたり
    AzurePubSubService-->>Client: メッセージを受け取ったり
    AzurePubSubService->>AzureFunctions:Message Event
    Client->>Client: ConnectionClose
    deactivate Client
    AzurePubSubService->>AzureFunctions: Disconnected Event
```