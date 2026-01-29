# Team Collaboration Guide - Hướng dẫn Làm việc Nhóm

## 👥 Team Information

**Project**: Pet Shop Management System (Hệ thống Quản lý Cửa hàng Thú cưng)  
**Team Size**: 4 members  
**IDE**: Visual Studio 2022  
**Framework**: .NET 8.0  
**Language**: C#

## 📋 Module Assignment

### Module 1: Pet Management (Quản lý Thú cưng)
**Assigned to**: Member 1  
**Responsibilities**:
- Manage pet inventory (add, edit, delete, view)
- Track pet details (species, breed, age, price)
- Search and filter pets
- Update availability status

**Files**:
- `Models/Pet.cs`
- `Services/Interfaces/IPetService.cs`
- `Services/Implementations/PetService.cs`

### Module 2: Customer Management (Quản lý Khách hàng)
**Assigned to**: Member 2  
**Responsibilities**:
- Manage customer database
- Track customer contact information
- View customer purchase history
- Search customers

**Files**:
- `Models/Customer.cs`
- `Services/Interfaces/ICustomerService.cs`
- `Services/Implementations/CustomerService.cs`

### Module 3: Product Management (Quản lý Sản phẩm)
**Assigned to**: Member 3  
**Responsibilities**:
- Manage product inventory (food, toys, accessories)
- Track stock levels
- Categorize products
- Update pricing

**Files**:
- `Models/Product.cs`
- `Services/Interfaces/IProductService.cs`
- `Services/Implementations/ProductService.cs`

### Module 4: Order Management (Quản lý Đơn hàng)
**Assigned to**: Member 4  
**Responsibilities**:
- Create and manage orders
- Calculate order totals
- Track order status
- Generate sales reports

**Files**:
- `Models/Order.cs`
- `Models/OrderItem.cs`
- `Services/Interfaces/IOrderService.cs`
- `Services/Implementations/OrderService.cs`

## 🔄 Git Workflow

### Daily Workflow

#### Morning
```bash
# 1. Update your local repository
git checkout main
git pull origin main

# 2. Switch to your feature branch
git checkout feature/your-module

# 3. Merge latest changes from main
git merge main
```

#### During Work
```bash
# 4. Make changes to your files
# Edit code in Visual Studio

# 5. Test your changes
dotnet build
dotnet run --project QuanLyThuCung.App

# 6. Commit frequently (every 1-2 hours)
git add .
git commit -m "Add: Description of what you added"
```

#### End of Day
```bash
# 7. Push your work to GitHub
git push origin feature/your-module

# 8. Create Pull Request if feature is complete
# Go to GitHub → Pull Requests → New PR
```

### Branch Naming Convention
- `feature/pet-management` - Member 1
- `feature/customer-management` - Member 2
- `feature/product-management` - Member 3
- `feature/order-management` - Member 4
- `bugfix/description` - For bug fixes
- `enhancement/description` - For improvements

### Commit Message Format
```
Type: Brief description

Types:
- Add: New feature or file
- Update: Modify existing feature
- Fix: Bug fix
- Refactor: Code refactoring
- Docs: Documentation changes

Examples:
✅ Add: Pet search functionality
✅ Update: Customer form validation
✅ Fix: Order total calculation error
✅ Refactor: Simplify product service methods
✅ Docs: Update README with API examples
```

## 🤝 Code Review Process

### Before Creating Pull Request
1. ✅ Test your code thoroughly
2. ✅ Build succeeds without errors
3. ✅ Code follows naming conventions
4. ✅ Add comments for complex logic
5. ✅ Update documentation if needed

### Creating Pull Request
1. Go to GitHub repository
2. Click "Pull requests" tab
3. Click "New pull request"
4. Select: `base: main` ← `compare: feature/your-branch`
5. Title: Clear description (e.g., "Add Pet Management Module")
6. Description: 
   - What was added/changed
   - How to test it
   - Screenshots if applicable
7. Assign reviewers (other team members)
8. Create pull request

### Reviewing Pull Requests
When assigned to review:
1. Read the description
2. Check out the branch locally:
   ```bash
   git fetch origin
   git checkout feature/branch-name
   ```
3. Test the code
4. Leave comments on specific lines if needed
5. Approve or Request changes

### Merging
- Only merge after at least 1 approval
- Resolve any conflicts before merging
- Delete branch after successful merge

## 📞 Communication

### Daily Standup (Suggested)
Every day, each member shares:
1. What did you do yesterday?
2. What will you do today?
3. Any blockers or issues?

### When to Ask for Help
- Stuck on a bug for more than 30 minutes
- Need clarification on requirements
- Merge conflicts you can't resolve
- Design decisions affecting other modules

### Communication Channels
- **GitHub Issues**: For bugs and feature requests
- **Pull Request Comments**: For code-specific discussions
- **Team Chat** (Zalo/Discord/Slack): For quick questions
- **Meetings**: For major decisions

## 🏗️ Development Standards

### Code Style
```csharp
// ✅ Good
public class PetService : IPetService
{
    private readonly List<Pet> _pets = new();
    
    public Pet? GetPetById(int id)
    {
        return _pets.FirstOrDefault(p => p.Id == id);
    }
}

// ❌ Bad
public class petservice
{
    List<Pet> pets = new List<Pet>();
    
    public Pet getpet(int i){
        return pets.Find(x=>x.Id==i);
    }
}
```

### Error Handling
```csharp
// Always validate input
public void AddPet(Pet pet)
{
    if (pet == null)
        throw new ArgumentNullException(nameof(pet));
        
    if (string.IsNullOrEmpty(pet.Name))
        throw new ArgumentException("Pet name is required");
        
    pet.Id = _nextId++;
    _pets.Add(pet);
}
```

### Comments
```csharp
// ✅ Good - Explains why
// Calculate discount for loyal customers (>5 orders)
if (customer.Orders.Count > 5)
    discount = 0.1m;

// ❌ Bad - States the obvious
// Check if count is greater than 5
if (customer.Orders.Count > 5)
```

## 🧪 Testing Guidelines

### Manual Testing Checklist
Before committing:
- [ ] Code compiles without errors
- [ ] Code compiles without warnings
- [ ] Happy path works (normal usage)
- [ ] Edge cases handled (null, empty, invalid data)
- [ ] Error messages are clear
- [ ] No hardcoded values

### Testing Your Module
```bash
# Build the solution
dotnet build

# Run the application
dotnet run --project QuanLyThuCung.App

# Test each feature:
# 1. View all items
# 2. Add new item
# 3. Search items
# 4. Update item
# 5. Delete item
```

## 🚨 Common Issues and Solutions

### Issue 1: Merge Conflicts
```bash
# When you get merge conflicts
git status  # See conflicted files

# Open conflicted files, you'll see:
<<<<<<< HEAD
your changes
=======
their changes
>>>>>>> branch-name

# Keep what you need, remove markers
# Then:
git add .
git commit -m "Resolve merge conflicts"
```

### Issue 2: Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Issue 3: Can't Push to GitHub
```bash
# Your branch is behind
git pull origin feature/your-branch
git push origin feature/your-branch
```

### Issue 4: Accidentally Committed to Main
```bash
# Create a new branch from current state
git checkout -b feature/emergency-fix

# Reset main branch
git checkout main
git reset --hard origin/main

# Continue work on feature branch
git checkout feature/emergency-fix
```

## 📅 Project Timeline (Example)

### Week 1: Setup & Basic Structure
- Day 1-2: Environment setup, Git workflow
- Day 3-4: Understand code structure
- Day 5: Implement basic CRUD for your module

### Week 2: Core Features
- Day 1-3: Complete your module features
- Day 4: Integration testing
- Day 5: Code review and fixes

### Week 3: Enhancement & Testing
- Day 1-2: Add advanced features
- Day 3-4: Testing and bug fixes
- Day 5: Documentation

### Week 4: Integration & Polish
- Day 1-2: Integrate all modules
- Day 3: User acceptance testing
- Day 4-5: Final fixes and presentation prep

## 📝 Meeting Agenda Template

### Weekly Team Meeting
**Date**: [Date]  
**Duration**: 30-60 minutes

**Agenda**:
1. Progress Update (5 min each member)
   - What was completed
   - Current work
   - Blockers
   
2. Code Review (10 min)
   - Review merged PRs
   - Discuss code quality issues
   
3. Integration Discussion (10 min)
   - How modules interact
   - Shared interfaces
   - Data flow
   
4. Planning (10 min)
   - Next week goals
   - Task assignment
   - Deadlines

5. Q&A (10 min)

## 🎯 Success Metrics

### Individual Performance
- [ ] Commits regularly (daily)
- [ ] Code passes review on first try
- [ ] Meets deadlines
- [ ] Helps other team members
- [ ] Documentation is clear

### Team Performance
- [ ] All modules completed on time
- [ ] Integration works smoothly
- [ ] No major bugs in production
- [ ] Good test coverage
- [ ] Clear documentation

## 📚 Resources for Learning

### C# & .NET
- [Microsoft C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [C# Tutorial for Beginners](https://www.youtube.com/watch?v=GhQdlIFylQ8)

### Git & GitHub
- [Git Handbook](https://guides.github.com/introduction/git-handbook/)
- [GitHub Flow](https://guides.github.com/introduction/flow/)

### Visual Studio
- [Visual Studio Tips & Tricks](https://docs.microsoft.com/en-us/visualstudio/ide/)

## 🎉 Team Guidelines

### Do's ✅
- Communicate openly and frequently
- Ask questions when unclear
- Help teammates when they're stuck
- Test your code before committing
- Write clear commit messages
- Review others' code constructively
- Document your code

### Don'ts ❌
- Don't commit broken code
- Don't push directly to main branch
- Don't ignore merge conflicts
- Don't work on others' modules without coordination
- Don't skip code reviews
- Don't commit IDE-specific files (.vs/, bin/, obj/)
- Don't leave TODO comments without creating issues

---

**Remember**: We're a team! Success comes from helping each other and working together. 🚀

**Questions?** Ask in the team chat or create a GitHub Issue.
