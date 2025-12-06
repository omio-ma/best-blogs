# Data Model: Lifestyle Blog Platform

**Date**: 2025-12-06
**Feature**: 001-blog-platform

## Overview

This document defines the database schema, entity relationships, and validation rules for the blog platform. The data model supports blog post management, commenting, categorization, and admin authentication.

---

## Entity Relationship Diagram

```
┌─────────────┐
│  Category   │
│             │
│ - id (PK)   │
│ - name      │
│ - description│
│ - createdAt │
└──────┬──────┘
       │
       │ 1:N
       │
┌──────▼──────────┐
│   Blog Post     │
│                 │
│ - id (PK)       │
│ - title         │
│ - content       │
│ - excerpt       │
│ - authorName    │
│ - categoryId(FK)│
│ - publishedAt   │
│ - createdAt     │
│ - updatedAt     │
│ - isDeleted     │
└──────┬──────────┘
       │
       │ 1:N
       │
┌──────▼──────────┐
│    Comment      │
│                 │
│ - id (PK)       │
│ - postId (FK)   │
│ - commenterName │
│ - commenterEmail│
│ - content       │
│ - isSpam        │
│ - createdAt     │
└─────────────────┘

┌─────────────────┐
│   Admin User    │
│                 │
│ - id (PK)       │
│ - email         │
│ - oauthProvider │
│ - oauthId       │
│ - displayName   │
│ - createdAt     │
└─────────────────┘
(no relationships - all admins have full permissions)
```

---

## Entities

### 1. Category

**Purpose**: Organize blog posts into topics (Lifestyle, Travel, Food, Personal, etc.)

**Fields**:

| Field | Type | Constraints | Description |
|-------|------|-------------|-------------|
| id | UUID | PRIMARY KEY | Unique identifier |
| name | VARCHAR(50) | NOT NULL, UNIQUE | Category display name |
| description | TEXT | NULLABLE | Optional category description |
| createdAt | TIMESTAMP | NOT NULL, DEFAULT NOW() | Record creation timestamp |

**Indexes**:
- Primary key on `id`
- Unique index on `name`

**Validation Rules**:
- `name`: 2-50 characters, alphanumeric + spaces
- `description`: 0-500 characters

**Business Rules**:
- Default "General" category (id='00000000-0000-0000-0000-000000000001') must always exist
- Cannot delete "General" category
- Deleting a category reassigns all posts to "General"

**TypeORM Entity**:
```typescript
@Entity('categories')
export class Category {
  @PrimaryGeneratedColumn('uuid')
  id: string;

  @Column({ type: 'varchar', length: 50, unique: true })
  name: string;

  @Column({ type: 'text', nullable: true })
  description: string | null;

  @CreateDateColumn()
  createdAt: Date;

  @OneToMany(() => BlogPost, post => post.category)
  posts: BlogPost[];
}
```

---

### 2. Blog Post

**Purpose**: Represent individual blog articles with content, metadata, and categorization

**Fields**:

| Field | Type | Constraints | Description |
|-------|------|-------------|-------------|
| id | UUID | PRIMARY KEY | Unique identifier |
| title | VARCHAR(200) | NOT NULL | Post title |
| content | TEXT | NOT NULL | Full post content (Markdown or rich text) |
| excerpt | VARCHAR(300) | NOT NULL | Short preview (first 150-200 words) |
| authorName | VARCHAR(100) | NOT NULL | Author display name |
| categoryId | UUID | FOREIGN KEY → categories(id) | Category reference |
| publishedAt | TIMESTAMP | NOT NULL, DEFAULT NOW() | Publication timestamp |
| createdAt | TIMESTAMP | NOT NULL, DEFAULT NOW() | Record creation timestamp |
| updatedAt | TIMESTAMP | NOT NULL, DEFAULT NOW() | Last update timestamp |
| isDeleted | BOOLEAN | NOT NULL, DEFAULT false | Soft delete flag |

**Indexes**:
- Primary key on `id`
- Index on `publishedAt DESC` (for homepage sorting)
- Index on `categoryId` (for category filtering)
- Index on `isDeleted` (for active posts queries)

**Validation Rules**:
- `title`: 5-200 characters
- `content`: 100-50,000 characters
- `excerpt`: Auto-generated from first 150-200 words if not provided
- `authorName`: 2-100 characters
- `categoryId`: Must reference existing category

**Business Rules**:
- Soft delete: set `isDeleted=true` instead of hard delete
- Auto-generate `excerpt` from `content` if not provided
- Default `categoryId` to "General" if not specified
- `publishedAt` can be set in the future (scheduled posts)

**TypeORM Entity**:
```typescript
@Entity('blog_posts')
export class BlogPost {
  @PrimaryGeneratedColumn('uuid')
  id: string;

  @Column({ type: 'varchar', length: 200 })
  title: string;

  @Column({ type: 'text' })
  content: string;

  @Column({ type: 'varchar', length: 300 })
  excerpt: string;

  @Column({ type: 'varchar', length: 100 })
  authorName: string;

  @Column({ type: 'uuid' })
  categoryId: string;

  @ManyToOne(() => Category, category => category.posts)
  @JoinColumn({ name: 'categoryId' })
  category: Category;

  @Column({ type: 'timestamp', default: () => 'CURRENT_TIMESTAMP' })
  publishedAt: Date;

  @CreateDateColumn()
  createdAt: Date;

  @UpdateDateColumn()
  updatedAt: Date;

  @Column({ type: 'boolean', default: false })
  isDeleted: boolean;

  @OneToMany(() => Comment, comment => comment.post)
  comments: Comment[];
}
```

---

### 3. Comment

**Purpose**: Store visitor comments on blog posts with spam filtering

**Fields**:

| Field | Type | Constraints | Description |
|-------|------|-------------|-------------|
| id | UUID | PRIMARY KEY | Unique identifier |
| postId | UUID | FOREIGN KEY → blog_posts(id) | Blog post reference |
| commenterName | VARCHAR(50) | NOT NULL | Commenter display name |
| commenterEmail | VARCHAR(255) | NOT NULL | Commenter email address |
| content | TEXT | NOT NULL | Comment text |
| isSpam | BOOLEAN | NOT NULL, DEFAULT false | Spam flag |
| createdAt | TIMESTAMP | NOT NULL, DEFAULT NOW() | Comment timestamp |

**Indexes**:
- Primary key on `id`
- Compound index on `(postId, createdAt DESC)` (for comment pagination)
- Partial index on `(postId) WHERE isSpam=false` (for fetching non-spam comments)

**Validation Rules**:
- `commenterName`: 2-50 characters
- `commenterEmail`: Valid email format, max 255 characters
- `content`: 10-1000 characters

**Business Rules**:
- Spam detection runs on submission, sets `isSpam=true` if detected
- Comments with `isSpam=true` are hidden from public display
- Rate limit: max 3 comments per email per 10 minutes
- Duplicate prevention: same content + email within 5 minutes

**TypeORM Entity**:
```typescript
@Entity('comments')
export class Comment {
  @PrimaryGeneratedColumn('uuid')
  id: string;

  @Column({ type: 'uuid' })
  postId: string;

  @ManyToOne(() => BlogPost, post => post.comments, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'postId' })
  post: BlogPost;

  @Column({ type: 'varchar', length: 50 })
  commenterName: string;

  @Column({ type: 'varchar', length: 255 })
  commenterEmail: string;

  @Column({ type: 'text' })
  content: string;

  @Column({ type: 'boolean', default: false })
  isSpam: boolean;

  @CreateDateColumn()
  createdAt: Date;
}
```

---

### 4. Admin User

**Purpose**: Store administrator credentials and OAuth information

**Fields**:

| Field | Type | Constraints | Description |
|-------|------|-------------|-------------|
| id | UUID | PRIMARY KEY | Unique identifier |
| email | VARCHAR(255) | NOT NULL, UNIQUE | Admin email address |
| oauthProvider | VARCHAR(20) | NOT NULL | OAuth provider (google, github) |
| oauthId | VARCHAR(255) | NOT NULL | Provider-specific user ID |
| displayName | VARCHAR(100) | NOT NULL | Admin display name |
| createdAt | TIMESTAMP | NOT NULL, DEFAULT NOW() | Account creation timestamp |

**Indexes**:
- Primary key on `id`
- Unique index on `email`
- Unique compound index on `(oauthProvider, oauthId)`

**Validation Rules**:
- `email`: Valid email format
- `oauthProvider`: enum ['google', 'github']
- `oauthId`: Non-empty string from OAuth provider
- `displayName`: 2-100 characters

**Business Rules**:
- All authenticated admins have full permissions (no role-based access control)
- OAuth profile data refreshed on each login
- First authenticated user becomes admin (no manual signup)

**TypeORM Entity**:
```typescript
@Entity('admin_users')
export class AdminUser {
  @PrimaryGeneratedColumn('uuid')
  id: string;

  @Column({ type: 'varchar', length: 255, unique: true })
  email: string;

  @Column({ type: 'varchar', length: 20 })
  oauthProvider: 'google' | 'github';

  @Column({ type: 'varchar', length: 255 })
  oauthId: string;

  @Column({ type: 'varchar', length: 100 })
  displayName: string;

  @CreateDateColumn()
  createdAt: Date;
}
```

---

## Database Migrations

### Initial Migration (001_initial_schema.ts)

```typescript
export class InitialSchema1701000000000 implements MigrationInterface {
  public async up(queryRunner: QueryRunner): Promise<void> {
    // Create categories table
    await queryRunner.query(`
      CREATE TABLE categories (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        name VARCHAR(50) NOT NULL UNIQUE,
        description TEXT,
        created_at TIMESTAMP NOT NULL DEFAULT NOW()
      );
    `);

    // Insert default "General" category
    await queryRunner.query(`
      INSERT INTO categories (id, name, description)
      VALUES ('00000000-0000-0000-0000-000000000001', 'General', 'Uncategorized posts');
    `);

    // Create blog_posts table
    await queryRunner.query(`
      CREATE TABLE blog_posts (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        title VARCHAR(200) NOT NULL,
        content TEXT NOT NULL,
        excerpt VARCHAR(300) NOT NULL,
        author_name VARCHAR(100) NOT NULL,
        category_id UUID NOT NULL REFERENCES categories(id),
        published_at TIMESTAMP NOT NULL DEFAULT NOW(),
        created_at TIMESTAMP NOT NULL DEFAULT NOW(),
        updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
        is_deleted BOOLEAN NOT NULL DEFAULT false
      );

      CREATE INDEX idx_blog_posts_published_at ON blog_posts(published_at DESC);
      CREATE INDEX idx_blog_posts_category_id ON blog_posts(category_id);
      CREATE INDEX idx_blog_posts_is_deleted ON blog_posts(is_deleted);
    `);

    // Create comments table
    await queryRunner.query(`
      CREATE TABLE comments (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        post_id UUID NOT NULL REFERENCES blog_posts(id) ON DELETE CASCADE,
        commenter_name VARCHAR(50) NOT NULL,
        commenter_email VARCHAR(255) NOT NULL,
        content TEXT NOT NULL,
        is_spam BOOLEAN NOT NULL DEFAULT false,
        created_at TIMESTAMP NOT NULL DEFAULT NOW()
      );

      CREATE INDEX idx_comments_post_created ON comments(post_id, created_at DESC);
      CREATE INDEX idx_comments_not_spam ON comments(post_id) WHERE is_spam = false;
    `);

    // Create admin_users table
    await queryRunner.query(`
      CREATE TABLE admin_users (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        email VARCHAR(255) NOT NULL UNIQUE,
        oauth_provider VARCHAR(20) NOT NULL,
        oauth_id VARCHAR(255) NOT NULL,
        display_name VARCHAR(100) NOT NULL,
        created_at TIMESTAMP NOT NULL DEFAULT NOW(),
        UNIQUE(oauth_provider, oauth_id)
      );

      CREATE INDEX idx_admin_users_email ON admin_users(email);
    `);
  }

  public async down(queryRunner: QueryRunner): Promise<void> {
    await queryRunner.query(`DROP TABLE IF EXISTS comments CASCADE;`);
    await queryRunner.query(`DROP TABLE IF EXISTS blog_posts CASCADE;`);
    await queryRunner.query(`DROP TABLE IF EXISTS admin_users CASCADE;`);
    await queryRunner.query(`DROP TABLE IF EXISTS categories CASCADE;`);
  }
}
```

---

## Data Seeding (Development)

### Seed Data for Testing

```typescript
// seeds/dev-seed.ts
export const seedDevData = async (dataSource: DataSource) => {
  const categoryRepo = dataSource.getRepository(Category);
  const postRepo = dataSource.getRepository(BlogPost);

  // Create additional categories
  const categories = await categoryRepo.save([
    { name: 'Lifestyle', description: 'Everyday life and personal experiences' },
    { name: 'Travel', description: 'Travel adventures and destinations' },
    { name: 'Food', description: 'Recipes and food experiences' },
    { name: 'Personal', description: 'Personal reflections and stories' },
  ]);

  // Create sample posts
  await postRepo.save([
    {
      title: 'Welcome to My Blog',
      content: 'This is the beginning of my blogging journey...',
      excerpt: 'This is the beginning of my blogging journey and I am excited to share my thoughts with you.',
      authorName: 'Admin',
      categoryId: categories[0].id,
    },
    {
      title: 'My First Travel Adventure',
      content: 'Last week I visited an amazing place...',
      excerpt: 'Last week I visited an amazing place and had incredible experiences.',
      authorName: 'Admin',
      categoryId: categories[1].id,
    },
  ]);
};
```

---

## Pagination Strategy

### Blog Posts Pagination

- **Strategy**: Offset-based pagination (simple, predictable)
- **Page Size**: 10 posts per page (homepage), configurable
- **Query Pattern**:
  ```sql
  SELECT * FROM blog_posts
  WHERE is_deleted = false AND published_at <= NOW()
  ORDER BY published_at DESC
  LIMIT 10 OFFSET 0;
  ```

### Comments Pagination

- **Strategy**: Offset-based pagination
- **Page Size**: 20 comments initially, 20 per "Load More"
- **Query Pattern**:
  ```sql
  SELECT * FROM comments
  WHERE post_id = $1 AND is_spam = false
  ORDER BY created_at DESC
  LIMIT 20 OFFSET 0;
  ```

---

## Next Steps

1. Generate API contracts (OpenAPI specs)
2. Implement TypeORM entities in backend
3. Create database migrations
4. Set up development seed data
