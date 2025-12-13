Feature: Browse Blog Posts
  As a visitor
  I want to browse blog posts
  So that I can read interesting lifestyle content

  Scenario: View homepage with recent posts
    Given the blog has 15 published posts
    When I visit the homepage
    Then I should see 10 blog posts
    And I should see a "Load More" button
    And the posts should be ordered by published date (newest first)

  Scenario: Load more posts
    Given the blog has 15 published posts
    And I am on the homepage showing 10 posts
    When I click the "Load More" button
    Then I should see 5 additional posts
    And the "Load More" button should disappear

  Scenario: Filter posts by category
    Given the blog has 5 posts in "Travel" category
    And the blog has 3 posts in "Food" category
    When I filter by "Travel" category
    Then I should see 5 blog posts
    And all posts should be in "Travel" category

  Scenario: View single blog post
    Given a blog post exists with title "My Lifestyle Journey"
    When I view the post details
    Then I should see the post title "My Lifestyle Journey"
    And I should see the full post content
    And I should see the author name
    And I should see the published date
    And I should see the category
