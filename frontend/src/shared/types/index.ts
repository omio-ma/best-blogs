// Common API response types
export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  message?: string;
  errors?: string[];
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

// Domain types will be added in Phase 3
export interface BlogPost {
  id: string;
  title: string;
  content: string;
  excerpt: string;
  authorName: string;
  categoryId: string;
  categoryName?: string;
  publishedAt: string;
  createdAt: string;
  updatedAt?: string;
}

export interface Comment {
  id: string;
  postId: string;
  commenterName: string;
  commenterEmail: string;
  content: string;
  isSpam: boolean;
  createdAt: string;
}

export interface Category {
  id: string;
  name: string;
  description?: string;
  postCount?: number;
}
