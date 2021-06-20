``` mermaid
sequenceDiagram
    Front->>Server: アクセス要求
    Server->>AzurePubSubService: SDKを介して接続情報など作成
    AzurePubSubService-->>Server: アクセストークン返却
    Server-->>Front: アクセストークン付与したwssURLを返却
    Front->>Front: wssURLを使ってConnectionOpen
    activate Front
    Front->>AzurePubSubService: メッセージをぶん投げたり
    AzurePubSubService-->>Front: メッセージを受け取ったり
    AzurePubSubService->>AzureFunctions:トリガーでメッセージを受け取ったり
    Front->>Front: ConnectionClose
    deactivate Front
```