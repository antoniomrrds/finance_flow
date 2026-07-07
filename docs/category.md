# Get Category By Id

## Goal

Retrieve a category by its identifier.

---

## Request

### Endpoint

| Method | Route |
|--------|-------|
| GET | `/api/v1/categories/{id}` |

### Route Parameters

| Name | Type | Required |
|------|------|----------|
| id | Guid | ✅ |

---
## Main Flow

1. ⛔️ Authenticate the user.
2. ⛔️ Load the category by its identifier.
3. ⛔️ Map the category to the response.
4. ⛔️ Return `200 OK` with the category.
---

## Responses

| Status | Description | Progress |
|--------|-------------|----------|
| 200 OK | Category retrieved successfully. | ⛔️ |
| 401 Unauthorized | The user is not authenticated. | ⛔️ |
| 404 Not Found | The category does not exist. | ⛔️ |
| 500 Internal Server Error | An unexpected error occurred. | ⛔️ |

---

## Response

### 200 OK

```json
{
  "id": "6d9b7a0d-0b4b-4a88-b1e5-18c0ef9b0f2f",
  "name": "Electronics",
  "description": "Electronic devices and accessories",
  "createdAt": "2026-07-07T12:00:00Z",
  "updatedAt": "2026-07-07T12:00:00Z"
}
```