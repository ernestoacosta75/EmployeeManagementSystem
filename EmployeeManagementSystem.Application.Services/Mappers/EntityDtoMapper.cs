using Microsoft.Extensions.Options;

namespace EmployeeManagementSystem.Application.Services.Mappers
{
    public class EntityDtoMapper<TEntity, TDto>
    where TEntity : class, new()
    where TDto : class, new()
    {
        // Entity --> Dto
        public IEnumerable<TDto> ConstructFromEntitiesList(IEnumerable<TEntity> entities,
            Func<TEntity, TDto> mapToDto)
        {
            ArgumentNullException.ThrowIfNull(entities);
            ArgumentNullException.ThrowIfNull(mapToDto);

            return entities.Select(mapToDto);
        }

        public TDto ConstructFromEntity(TEntity entity, Func<TEntity, TDto> mapToDto)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(mapToDto);

            return mapToDto(entity);
        }

        // Dto --> Entity
        public IEnumerable<TEntity> ConstructFromDtoList(IEnumerable<TDto> dtoList,
            Func<TDto, TEntity> mapToEntity)
        {
            ArgumentNullException.ThrowIfNull(dtoList);
            ArgumentNullException.ThrowIfNull(mapToEntity);

            return dtoList.Select(mapToEntity);
        }

        public TEntity ConstructFromDto(TDto dto, Func<TDto, TEntity> mapToEntity)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(mapToEntity);

            return mapToEntity(dto);
        }
    }
}
